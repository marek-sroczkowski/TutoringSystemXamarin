using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Models.SubjectDtos
{
    public class NewSubjectDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public SubjectPlace Place { get; set; }
        public SubjectCategory Category { get; set; }

        public NewSubjectDto()
        {
        }

        public NewSubjectDto(string name, string description, SubjectPlace place, SubjectCategory category)
        {
            Name = name;
            Description = description;
            Place = place;
            Category = category;
        }
    }
}
