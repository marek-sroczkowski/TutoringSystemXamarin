using System.Collections.Generic;
using TutoringSystemMobile.Models.TutorDtos;

namespace TutoringSystemMobile.Models.StudentDtos
{
    public class StudentDetailsDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double HourlRate { get; set; }
        public ICollection<TutorDto> Tutors { get; set; }
    }
}
