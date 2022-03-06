using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.Dtos.Tutor;

namespace TutoringSystemMobile.Models.Dtos.Subject
{
    public class SubjectDetailsDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public SubjectPlace Place { get; set; }
        public SubjectCategory Category { get; set; }
        public TutorDto Tutor { get; set; }
    }
}
