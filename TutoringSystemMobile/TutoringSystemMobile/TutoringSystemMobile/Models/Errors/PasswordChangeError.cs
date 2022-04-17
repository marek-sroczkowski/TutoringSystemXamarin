using System.Collections.Generic;
using System.Text;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Models.Errors
{
    public class PasswordChangeError
    {
        public IEnumerable<string> PasswordErrors { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder($"{ToastConstans.PasswordChangeFailed}\n");

            foreach (var error in PasswordErrors)
            {
                string err = error.GetPasswordChangeError() switch
                {
                    WrongPasswordStatus.PasswordsVary => ToastConstans.PasswordsVary,
                    WrongPasswordStatus.NotMeetRequirements => ToastConstans.NotMeetRequirementsPassword,
                    WrongPasswordStatus.DuplicateOfOld => ToastConstans.DuplicateOfOldPassword,
                    WrongPasswordStatus.InvalidOldPassword => ToastConstans.InvalidOldPassword,
                    _ => ToastConstans.InternalError
                };

                builder.AppendLine(err);
            }

            return builder.ToString();
        }
    }
}