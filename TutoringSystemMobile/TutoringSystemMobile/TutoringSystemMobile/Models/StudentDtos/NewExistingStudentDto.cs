namespace TutoringSystemMobile.Models.StudentDtos
{
    public class NewExistingStudentDto
    {
        public string Username { get; set; }
        public double HourlRate { get; set; }
        public string Note { get; set; }

        public NewExistingStudentDto()
        {
        }

        public NewExistingStudentDto(string username, double hourlRate, string note)
        {
            Username = username;
            HourlRate = hourlRate;
            Note = note;
        }
    }
}