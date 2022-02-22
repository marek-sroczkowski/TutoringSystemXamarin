namespace TutoringSystemMobile.Models.Dtos.Account
{
    public class RegisterStudentDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double HourlRate { get; set; }
        public string Note { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public RegisterStudentDto()
        {
        }

        public RegisterStudentDto(string firstName, string lastName, double hourlRate, string note, string username, string password, string confirmPassword)
        {
            FirstName = firstName;
            LastName = lastName;
            HourlRate = hourlRate;
            Note = note;
            Username = username;
            Password = password;
            ConfirmPassword = confirmPassword;
        }
    }
}