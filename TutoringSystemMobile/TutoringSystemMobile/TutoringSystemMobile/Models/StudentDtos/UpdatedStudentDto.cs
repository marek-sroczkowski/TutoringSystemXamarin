namespace TutoringSystemMobile.Models.StudentDtos
{
    public class UpdatedStudentDto
    {
        public long StudentId { get; set; }
        public double HourRate { get; set; }
        public string Note { get; set; }

        public UpdatedStudentDto()
        {
        }

        public UpdatedStudentDto(long studentId, double hourRate, string note)
        {
            StudentId = studentId;
            HourRate = hourRate;
            Note = note;
        }
    }
}
