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
using Xamarin.Essentials;
using Xamarin.Forms;
using TutoringSystemMobile.Helpers;

[assembly: Dependency(typeof(SingleReservationService))]
namespace TutoringSystemMobile.Services.Web
{
    public class SingleReservationService : ISingleReservationService
    {
        private readonly string baseUrl;

        public SingleReservationService()
        {
            baseUrl = Settings.BaseApiUrl + "reservation/single";
        }

        public async Task<long> AddReservationByStudentAsync(NewStudentSingleReservationDto newReservation)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .AppendPathSegment("student")
                .WithOAuthBearerToken(token)
                .PostJsonAsync(newReservation);

            if (response.StatusCode != 201)
                return -1;

            string location = response.Headers.FirstOrDefault("location");

            return location is null ? -1 : location.GetIdByLocation();
        }

        public async Task<long> AddReservationByTutorAsync(NewTutorSingleReservationDto newReservation)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .AppendPathSegment("tutor")
                .WithOAuthBearerToken(token)
                .PostJsonAsync(newReservation);

            if (response.StatusCode != 201)
                return -1;

            string location = response.Headers.FirstOrDefault("location");

            return location is null ? -1 : location.GetIdByLocation();
        }

        public async Task<bool> DeleteReservationAsync(long reservationId)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .AppendPathSegment(reservationId)
                .WithOAuthBearerToken(token)
                .DeleteAsync();

            return response.StatusCode == 204;
        }

        public async Task<ReservationDetailsDto> GetReservationByIdAsync(long reservationId)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .AppendPathSegment(reservationId)
                .WithOAuthBearerToken(token)
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<ReservationDetailsDto>() : new ReservationDetailsDto();
        }

        public async Task<ReservationsCollectionDto> GetReservationsByStudentAsync(ReservationParameters parameters)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .AppendPathSegment("student")
                .SetQueryParams(parameters)
                .WithOAuthBearerToken(token)
                .GetAsync();

            return await GetReservationsAsync(response);
        }

        public async Task<ReservationsCollectionDto> GetReservationsByTutorAsync(ReservationParameters parameters)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .AppendPathSegment("tutor")
                .SetQueryParams(parameters)
                .WithOAuthBearerToken(token)
                .GetAsync();

            return await GetReservationsAsync(response);
        }

        public async Task<bool> UpdateStudentReservationAsync(UpdatedStudentReservationDto updatedReservation)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .AppendPathSegment("student")
                .WithOAuthBearerToken(token)
                .PutJsonAsync(updatedReservation);

            return response.StatusCode == 204;
        }

        public async Task<bool> UpdateTutorReservationAsync(UpdatedTutorReservationDto updatedReservation)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .AppendPathSegment("tutor")
                .WithOAuthBearerToken(token)
                .PutJsonAsync(updatedReservation);

            return response.StatusCode == 204;
        }

        private static async Task<ReservationsCollectionDto> GetReservationsAsync(IFlurlResponse response)
        {
            var reservations = response.StatusCode == 200 ?
                            await response.GetJsonAsync<IEnumerable<ReservationDto>>() :
                            new List<ReservationDto>();

            var pagination = response.StatusCode == 200 ?
                JsonConvert.DeserializeObject<PaginationMetadata>(response.Headers.FirstOrDefault("X-Pagination")) :
                new PaginationMetadata();

            return new ReservationsCollectionDto { Reservations = reservations, Pagination = pagination };
        }
    }
}