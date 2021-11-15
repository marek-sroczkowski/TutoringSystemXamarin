namespace TutoringSystemMobile.Models.StudentDtos
{
    public class UpdatedStudentDto
    {
        public long StudentId { get; set; }
        public double HourRate { get; set; }
        public string Note { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public UpdatedStudentDto()
        {
        }

        public UpdatedStudentDto(long studentId, double hourRate, string note, string firstName, string lastName)
        {
            StudentId = studentId;
            HourRate = hourRate;
            Note = note;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
