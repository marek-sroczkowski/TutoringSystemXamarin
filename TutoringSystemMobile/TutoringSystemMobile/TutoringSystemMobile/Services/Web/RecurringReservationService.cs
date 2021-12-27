using Flurl.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.Pagination;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Models.ReservationDtos;
using TutoringSystemMobile.Services.Interfaces;
using Xamarin.Essentials;

namespace TutoringSystemMobile.Services.Web
{
    public class RecurringReservationService : IRecurringReservationService
    {
        private readonly string baseUrl;

        public RecurringReservationService()
        {
            baseUrl = AppSettingsManager.Settings["BaseApiUrl"] + "reservation/recurring";
        }

        public async Task<long> AddReservationByStudentAsync(NewStudentRecurringReservationDto newReservation)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .AppendPathSegment("student")
                .WithOAuthBearerToken(token)
                .PostJsonAsync(newReservation);

            return GetNewReservationId(response);
        }

        public async Task<long> AddReservationByTutorAsync(NewTutorRecurringReservationDto newReservation)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .AppendPathSegment("tutor")
                .WithOAuthBearerToken(token)
                .PostJsonAsync(newReservation);

            return GetNewReservationId(response);
        }

        public async Task<bool> DeleteReservationAsync(long reservationId, RecurringReservationRemovingMode mode)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .AppendPathSegment(reservationId)
                .SetQueryParam(mode.ToString())
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

        private static long GetNewReservationId(IFlurlResponse response)
        {
            if (response.StatusCode != 201)
                return -1;

            string location = response.Headers.FirstOrDefault("location");

            return location is null ? -1 : location.GetIdByLocation();
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
                JsonConvert.DeserializeObject<PaginationMetadataDto>(response.Headers.FirstOrDefault("X-Pagination")) :
                new PaginationMetadataDto();

            return new ReservationsCollectionDto { Reservations = reservations, Pagination = pagination };
        }
    }
}