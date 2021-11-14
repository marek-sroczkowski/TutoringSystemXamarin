namespace TutoringSystemMobile.Models.StudentDtos
{
    public class NewExistingStudentDto
    {
        public string Username { get; set; }
        public double HourRate { get; set; }
        public string Note { get; set; }

        public NewExistingStudentDto()
        {
        }

        public NewExistingStudentDto(string username, double hourlRate, string note)
        {
            Username = username;
            HourRate = hourlRate;
            Note = note;
        }
    }
}