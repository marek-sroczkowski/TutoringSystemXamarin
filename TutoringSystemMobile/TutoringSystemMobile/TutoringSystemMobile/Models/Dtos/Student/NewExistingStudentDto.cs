namespace TutoringSystemMobile.Models.Dtos.Student
{
    public class NewExistingStudentDto
    {
        public long StudentId { get; set; }
        public double HourRate { get; set; }
        public string Note { get; set; }

        public NewExistingStudentDto()
        {
        }

        public NewExistingStudentDto(long studentId, double hourlRate, string note)
        {
            StudentId = studentId;
            HourRate = hourlRate;
            Note = note;
        }
    }
}