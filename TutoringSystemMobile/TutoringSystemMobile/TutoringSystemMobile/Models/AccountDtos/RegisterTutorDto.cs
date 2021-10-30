namespace TutoringSystemMobile.Models.AccountDtos
{
    public class RegisterTutorDto
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public RegisterTutorDto()
        {
        }

        public RegisterTutorDto(string username, string firstName, string email, string password, string confirmPassword)
        {
            Username = username;
            FirstName = firstName;
            Email = email;
            Password = password;
            ConfirmPassword = confirmPassword;
        }
    }
}
