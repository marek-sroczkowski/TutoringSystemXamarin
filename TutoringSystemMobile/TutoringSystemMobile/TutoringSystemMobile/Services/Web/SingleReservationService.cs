using Flurl.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Models.Pagination;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Models.Dtos.Reservation;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Web;
using Xamarin.Forms;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Constans;

[assembly: Dependency(typeof(SingleReservationService))]
namespace TutoringSystemMobile.Services.Web
{
    public class SingleReservationService : ISingleReservationService
    {
        private readonly string baseUrl;

        public SingleReservationService()
        {
            baseUrl = $"{Settings.BaseApiUrl}{ServicesConstans.Reservation}/{ServicesConstans.Single}";
        }

        public async Task<long> AddReservationByStudentAsync(NewStudentSingleReservationDto newReservation)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegment(ServicesConstans.Student)
                .PostJsonAsync(newReservation);

            string location = response.Headers.FirstOrDefault(ServicesConstans.Location);

            return location is null ? -1 : location.GetIdByLocation();
        }

        public async Task<long> AddReservationByTutorAsync(NewTutorSingleReservationDto newReservation)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegment(ServicesConstans.Tutor)
                .PostJsonAsync(newReservation);

            string location = response.Headers.FirstOrDefault(ServicesConstans.Location);

            return location is null ? -1 : location.GetIdByLocation();
        }

        public async Task<bool> RemoveReservationAsync(long reservationId)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegment(reservationId)
                .DeleteAsync();

            return response.StatusCode == 204;
        }

        public async Task<ReservationDetailsDto> GetReservationByIdAsync(long reservationId)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegment(reservationId)
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<ReservationDetailsDto>() : new ReservationDetailsDto();
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

        public async Task<bool> UpdateStudentReservationAsync(UpdatedStudentReservationDto updatedReservation)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegment(ServicesConstans.Student)
                .PutJsonAsync(updatedReservation);

            return response.StatusCode == 204;
        }

        public async Task<bool> UpdateTutorReservationAsync(UpdatedTutorReservationDto updatedReservation)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegment(ServicesConstans.Tutor)
                .PutJsonAsync(updatedReservation);

            return response.StatusCode == 204;
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