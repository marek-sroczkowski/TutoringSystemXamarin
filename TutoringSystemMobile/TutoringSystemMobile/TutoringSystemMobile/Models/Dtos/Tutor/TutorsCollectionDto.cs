using System.Collections.Generic;
using TutoringSystemMobile.Models.Pagination;

namespace TutoringSystemMobile.Models.Dtos.Tutor
{
    public class TutorsCollectionDto
    {
        public IEnumerable<TutorSimpleDto> Tutors { get; set; }
        public PaginationMetadata Pagination { get; set; }
    }
}