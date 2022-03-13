namespace TutoringSystemMobile.Models.Dtos.Account
{
    public class RegisteredStudentDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public RegisteredStudentDto()
        {
        }

        public RegisteredStudentDto(string email, string password, string confirmPassword)
        {
            Email = email;
            Password = password;
            ConfirmPassword = confirmPassword;
        }
    }
}