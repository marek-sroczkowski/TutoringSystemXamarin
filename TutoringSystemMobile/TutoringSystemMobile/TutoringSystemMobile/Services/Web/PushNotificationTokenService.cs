using Flurl.Http;
using Plugin.FirebasePushNotification;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.Dtos.PushNotificationToken;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Web;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(PushNotificationTokenService))]
namespace TutoringSystemMobile.Services.Web
{
    public class PushNotificationTokenService : IPushNotificationTokenService
    {
        private readonly string baseUrl;

        public PushNotificationTokenService()
        {
            baseUrl = AppSettingsManager.Settings["BaseApiUrl"] + "pushNotification/token";
        }

        public async Task<bool> PutTokenAsync()
        {
            string token = await SecureStorage.GetAsync("token");
            var deviceToken = CrossFirebasePushNotification.Current.Token;
            var response = await baseUrl
                .WithOAuthBearerToken(token)
                .AllowAnyHttpStatus()
                .PutJsonAsync(new PushNotificationTokenDto { RegistrationToken = deviceToken });

            return response.StatusCode == 204;
        }
    }
}