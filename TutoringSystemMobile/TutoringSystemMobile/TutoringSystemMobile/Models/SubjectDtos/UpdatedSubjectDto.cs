using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Models.SubjectDtos
{
    public class UpdatedSubjectDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public SubjectPlace Place { get; set; }
        public SubjectCategory Category { get; set; }

        public UpdatedSubjectDto()
        {
        }

        public UpdatedSubjectDto(long id, string name, string description, SubjectPlace place, SubjectCategory category)
        {
            Id = id;
            Name = name;
            Description = description;
            Place = place;
            Category = category;
        }
    }
}
