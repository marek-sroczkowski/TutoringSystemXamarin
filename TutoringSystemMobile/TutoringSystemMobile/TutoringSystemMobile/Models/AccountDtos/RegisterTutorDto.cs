namespace TutoringSystemMobile.Models.AccountDtos
{
    public class RegisterTutorDto
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public RegisterTutorDto()
        {
        }

        public RegisterTutorDto(string username, string firstName, string lastName, string email, string password, string confirmPassword)
        {
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            ConfirmPassword = confirmPassword;
        }
    }
}
