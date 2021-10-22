using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.TutorDtos;

namespace TutoringSystemMobile.Models.SubjectDtos
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
