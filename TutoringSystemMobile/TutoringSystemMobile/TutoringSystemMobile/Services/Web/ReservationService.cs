using Flurl.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.Pagination;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Models.Dtos.Reservation;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Web;
using Xamarin.Forms;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Constans;

[assembly: Dependency(typeof(ReservationService))]
namespace TutoringSystemMobile.Services.Web
{
    public class ReservationService : IReservationService
    {
        private readonly string baseUrl;

        public ReservationService()
        {
            baseUrl = Settings.BaseApiUrl + ServicesConstans.Reservation;
        }

        public async Task<ReservationsCollectionDto> GetReservationsByStudentAsync(ReservationParameters parameters)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegment(ServicesConstans.Student)
                .SetQueryParams(parameters)
                .GetAsync();

            return await GetReservationsAsync(response);
        }

        public async Task<ReservationsCollectionDto> GetReservationsByTutorAsync(ReservationParameters parameters)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegment(ServicesConstans.Tutor)
                .SetQueryParams(parameters)
                .GetAsync();

            return await GetReservationsAsync(response);
        }

        public async Task<ReservationDetailsDto> GetReservationByIdAsync(long reservationId)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegment(reservationId)
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<ReservationDetailsDto>() : new ReservationDetailsDto();
        }

        private static async Task<ReservationsCollectionDto> GetReservationsAsync(IFlurlResponse response)
        {
            var reservations = response.StatusCode == 200
                ? await response.GetJsonAsync<IEnumerable<ReservationDto>>()
                : new List<ReservationDto>();

            var pagination = response.StatusCode == 200
                ? JsonConvert.DeserializeObject<PaginationMetadata>(response.Headers.FirstOrDefault(ServicesConstans.XPagination))
                : new PaginationMetadata();

            return new ReservationsCollectionDto { Reservations = reservations, Pagination = pagination };
        }
    }
}