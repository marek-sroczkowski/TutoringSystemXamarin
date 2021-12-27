using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.Enums;
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

        public Task<ReservationDto> AddReservationByStudentAsync(NewStudentRecurringReservationDto newReservation)
        {
            throw new NotImplementedException();
        }

        public Task<ReservationDto> AddReservationByTutorAsync(NewTutorRecurringReservationDto newReservation)
        {
            throw new NotImplementedException();
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

        public Task<ReservationDetailsDto> GetReservationByIdAsync(long reservationId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<ReservationDto>> GetReservationsByStudentAsync(ReservationParameters parameters)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<ReservationDto>> GetReservationsByTutorAsync(ReservationParameters parameters)
        {
            throw new NotImplementedException();
        }
    }
}
