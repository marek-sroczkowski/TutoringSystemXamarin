﻿using Flurl.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.Pagination;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Models.ReservationDtos;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Web;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(ReservationService))]
namespace TutoringSystemMobile.Services.Web
{
    public class ReservationService : IReservationService
    {
        private readonly string baseUrl;

        public ReservationService()
        {
            baseUrl = AppSettingsManager.Settings["BaseApiUrl"] + "reservation";
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