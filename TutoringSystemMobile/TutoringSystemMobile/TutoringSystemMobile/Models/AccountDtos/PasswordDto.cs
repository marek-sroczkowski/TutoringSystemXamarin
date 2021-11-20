namespace TutoringSystemMobile.Models.AccountDtos
{
    public class PasswordDto
    {
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string OldPassword { get; set; }

        public PasswordDto()
        {
        }

        public PasswordDto(string newPassword, string confirmPassword, string oldPassword)
        {
            NewPassword = newPassword;
            ConfirmPassword = confirmPassword;
            OldPassword = oldPassword;
        }
    }
}