using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.Enums;
using Xamarin.Essentials;

namespace TutoringSystemMobile.Extensions
{
    public static class StringExtensions
    {
        public static long GetIdByLocation(this string location)
        {
            var locationItems = location?.Split('/');
            if (locationItems is null || locationItems.Length == 0)
                return -1;

            var id = locationItems.LastOrDefault();

            if (!long.TryParse(id, out long result))
                return -1;

            return result;
        }

        public static double ToDouble(this string number)
        {
            if (!double.TryParse(number, out double result))
                return 0;

            return result;
        }

        public static bool IsEmpty(this string property)
        {
            return string.IsNullOrEmpty(property) ||
                string.IsNullOrWhiteSpace(property) ||
                property.Trim(' ').Equals("-");
        }

        public static AccountStatus GetAccountStatus(this string status)
        {
            return string.IsNullOrEmpty(status)
                ? AccountStatus.LoggedOut
                : (AccountStatus)Enum.Parse(typeof(AccountStatus), status);
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
    }
}