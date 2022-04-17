using System.Collections.Generic;
using System.Text;
using TutoringSystemMobile.Constans;

namespace TutoringSystemMobile.Models.Errors
{
    public class RegisterErrors
    {
        public IEnumerable<string> Email { get; set; }
        public IEnumerable<string> Password { get; set; }
        public IEnumerable<string> Username { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder($"{ToastConstans.RegistrationFailed}\n");

            if (Email != null)
            {
                builder.AppendLine(ToastConstans.TakenEmail);
            }
            if (Username != null)
            {
                builder.AppendLine(ToastConstans.TakenLogin);
            }
            if (Password != null)
            {
                builder.AppendLine(ToastConstans.IncorrectPassword);
            }

            return builder.ToString();
        }
    }
}