using Flurl.Http;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Services.Interceptors;
using Xamarin.Essentials;

namespace TutoringSystemMobile.Extensions
{
    public static class StringExtensions
    {
        public static long GetIdByLocation(this string location)
        {
            var locationItems = location?.Split('/');
            if (locationItems is null || locationItems.Length == 0)
            {
                return -1;
            }

            var id = locationItems.LastOrDefault();

            return !long.TryParse(id, out long result) ? -1 : result;
        }

        public static double ToDouble(this string number)
        {
            return !double.TryParse(number, out double result) ? 0 : result;
        }

        public static bool IsEmpty(this string property)
        {
            return string.IsNullOrEmpty(property) ||
                string.IsNullOrWhiteSpace(property) ||
                property.Trim(' ').Equals("-");
        }

        public static WrongPasswordStatus GetPasswordChangeError(this string error)
        {
            return (WrongPasswordStatus)Enum.Parse(typeof(WrongPasswordStatus), error);
        }

        public static bool IsValidEmail(this string email)
        {
            if (email.Trim().EndsWith("."))
            {
                return false;
            }

            try
            {
                return new MailAddress(email).Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static DateTime ToDateTime(this string date)
        {
            return date is null ? default : DateTime.Parse(date);
        }

        public static async Task<IFlurlRequest> BaseRequest(this string url)
        {
            var handler = new TokenRefresher();
            var httpClient = new HttpClient(handler);
            IFlurlClient client = new FlurlClient(httpClient);
            string token = await SecureStorage.GetAsync(SecureStorageConstans.JwtToken);

            return url
                .WithClient(client)
                .AllowAnyHttpStatus()
                .WithOAuthBearerToken(token);
        }
    }
}