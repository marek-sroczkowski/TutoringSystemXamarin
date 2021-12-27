using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Models.SubjectDtos
{
    public class SubjectDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public SubjectPlace Place { get; set; }
        public SubjectCategory Category { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}