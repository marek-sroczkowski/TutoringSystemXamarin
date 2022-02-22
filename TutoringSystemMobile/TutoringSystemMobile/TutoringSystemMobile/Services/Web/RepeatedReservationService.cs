using Flurl.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.Dtos.Reservation;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Web;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(RepeatedReservationService))]
namespace TutoringSystemMobile.Services.Web
{
    public class RepeatedReservationService : IRepeatedReservationService
    {
        private readonly string baseUrl;

        public RepeatedReservationService()
        {
            baseUrl = AppSettingsManager.Settings["BaseApiUrl"] + "reservation/repeated";
        }

        public async Task<IEnumerable<RepeatedReservationDto>> GetReservationsByStudentAsync()
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .AppendPathSegment("student")
                .WithOAuthBearerToken(token)
                .GetAsync();

            return response.StatusCode == 200
                ? await response.GetJsonAsync<IEnumerable<RepeatedReservationDto>>()
                : new List<RepeatedReservationDto>();
        }

        public async Task<IEnumerable<RepeatedReservationDto>> GetReservationsByTutorAsync()
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .AppendPathSegment("tutor")
                .WithOAuthBearerToken(token)
                .GetAsync();

            return response.StatusCode == 200
                ? await response.GetJsonAsync<IEnumerable<RepeatedReservationDto>>()
                : new List<RepeatedReservationDto>();
        }
    }
}
