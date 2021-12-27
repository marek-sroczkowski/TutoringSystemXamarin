using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Models.ReservationDtos;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Web;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(SingleReservationService))]
namespace TutoringSystemMobile.Services.Web
{
    public class SingleReservationService : ISingleReservationService
    {
        private readonly string baseUrl;

        public SingleReservationService()
        {
            baseUrl = AppSettingsManager.Settings["BaseApiUrl"] + "reservation/single";
        }

        public Task<ReservationDto> AddReservationByStudentAsync(NewStudentSingleReservationDto newReservation)
        {
            throw new NotImplementedException();
        }

        public Task<ReservationDto> AddReservationByTutorAsync(NewTutorSingleReservationDto newReservation)
        {
            throw new NotImplementedException();
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

        public Task<bool> UpdateStudentReservationAsync(UpdatedStudentReservationDto updatedReservation)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateTutorReservationAsync(UpdatedTutorReservationDto updatedReservation)
        {
            throw new NotImplementedException();
        }
    }
}
