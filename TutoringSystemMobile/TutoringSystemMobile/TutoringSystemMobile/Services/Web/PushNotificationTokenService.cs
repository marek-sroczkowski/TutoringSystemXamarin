using Flurl.Http;
using Plugin.FirebasePushNotification;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Models.Dtos.PushNotificationToken;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Web;
using Xamarin.Forms;

[assembly: Dependency(typeof(PushNotificationTokenService))]
namespace TutoringSystemMobile.Services.Web
{
    public class PushNotificationTokenService : IPushNotificationTokenService
    {
        private readonly string baseUrl;

        public PushNotificationTokenService()
        {
            baseUrl = $"{Settings.BaseApiUrl}{ServicesConstans.PushNotification}/{ServicesConstans.Token}";
        }

        public async Task<bool> PutTokenAsync()
        {
            var deviceToken = CrossFirebasePushNotification.Current.Token;
            var token = new PushNotificationTokenDto { RegistrationToken = deviceToken };

            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .PutJsonAsync(token);

            return response.StatusCode == 204;
        }
    }
}