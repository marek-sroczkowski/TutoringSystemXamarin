namespace TutoringSystemMobile.Models.Dtos.Account
{
    public class NewStudentDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double HourlRate { get; set; }
        public string Note { get; set; }
        public string Username { get; set; }

        public NewStudentDto()
        {
        }

        public NewStudentDto(string firstName, string lastName, double hourlRate, string note, string username)
        {
            FirstName = firstName;
            LastName = lastName;
            HourlRate = hourlRate;
            Note = note;
            Username = username;
        }
    }
}