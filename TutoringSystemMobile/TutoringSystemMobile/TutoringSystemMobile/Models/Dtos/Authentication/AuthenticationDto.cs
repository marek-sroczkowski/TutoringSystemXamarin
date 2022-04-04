namespace TutoringSystemMobile.Models.Dtos.Authentication
{
    public class AuthenticationDto
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public AuthenticationDto()
        {
        }

        public AuthenticationDto(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}