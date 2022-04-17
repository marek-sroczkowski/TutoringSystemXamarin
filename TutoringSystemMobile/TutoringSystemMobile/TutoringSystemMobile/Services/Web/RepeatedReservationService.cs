using Flurl.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Models.Dtos.Reservation;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Web;
using Xamarin.Forms;

[assembly: Dependency(typeof(RepeatedReservationService))]
namespace TutoringSystemMobile.Services.Web
{
    public class RepeatedReservationService : IRepeatedReservationService
    {
        private readonly string baseUrl;

        public RepeatedReservationService()
        {
            baseUrl = $"{Settings.BaseApiUrl}{ServicesConstans.Reservation}/{ServicesConstans.Repeated}";
        }

        public async Task<IEnumerable<RepeatedReservationDto>> GetReservationsByStudentAsync()
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegment(ServicesConstans.Student)
                .GetAsync();

            return response.StatusCode == 200
                ? await response.GetJsonAsync<IEnumerable<RepeatedReservationDto>>()
                : new List<RepeatedReservationDto>();
        }

        public async Task<IEnumerable<RepeatedReservationDto>> GetReservationsByTutorAsync()
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegment(ServicesConstans.Tutor)
                .GetAsync();

            return response.StatusCode == 200
                ? await response.GetJsonAsync<IEnumerable<RepeatedReservationDto>>()
                : new List<RepeatedReservationDto>();
        }
    }
}