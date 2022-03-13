namespace TutoringSystemMobile.Models.Dtos.Account
{
    public class RegisteredTutorDto
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public RegisteredTutorDto()
        {
        }

        public RegisteredTutorDto(string username, string firstName, string lastName, string email, string password, string confirmPassword)
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