using System;

namespace TutoringSystemMobile.Models.StudentRequestDtos
{
    public class StudentRequestDto
    {
        private DateTime createdDate;

        public long Id { get; set; }
        public long StudentId { get; set; }
        public string StudentUsername { get; set; }
        public string StudentName { get; set; }
        public long TutorId { get; set; }
        public string TutorUsername { get; set; }
        public string TutorName { get; set; }
        public string CreatedDate
        {
            get => createdDate.ToShortDateString();
            set => createdDate = Convert.ToDateTime(value);
        }
    }
}