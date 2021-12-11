using System.Collections.Generic;
using TutoringSystemMobile.Models.Pagination;

namespace TutoringSystemMobile.Models.TutorDtos
{
    public class TutorsCollectionDto
    {
        public IEnumerable<TutorSimpleDto> Tutors { get; set; }
        public PaginationMetadataDto Pagination { get; set; }
    }
}