using System.Collections.Generic;
using TutoringSystemMobile.Models.Pagination;

namespace TutoringSystemMobile.Models.StudentDtos
{
    public class StudentsCollectionDto
    {
        public IEnumerable<StudentSimpleDto> Students { get; set; }
        public PaginationMetadataDto Pagination { get; set; }
    }
}