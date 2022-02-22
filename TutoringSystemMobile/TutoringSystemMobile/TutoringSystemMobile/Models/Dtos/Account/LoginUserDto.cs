namespace TutoringSystemMobile.Models.Dtos.Account
{
    public class LoginUserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public LoginUserDto()
        {
        }

        public LoginUserDto(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}