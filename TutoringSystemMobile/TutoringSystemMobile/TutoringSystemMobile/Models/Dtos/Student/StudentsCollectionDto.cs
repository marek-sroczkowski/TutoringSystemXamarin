using System.Collections.Generic;
using TutoringSystemMobile.Models.Pagination;

namespace TutoringSystemMobile.Models.Dtos.Student
{
    public class StudentsCollectionDto
    {
        public IEnumerable<StudentSimpleDto> Students { get; set; }
        public PaginationMetadata Pagination { get; set; }
    }
}