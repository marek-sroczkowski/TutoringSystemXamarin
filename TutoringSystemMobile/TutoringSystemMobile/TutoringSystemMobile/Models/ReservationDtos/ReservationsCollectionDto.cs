using System.Collections.Generic;
using TutoringSystemMobile.Models.Pagination;

namespace TutoringSystemMobile.Models.ReservationDtos
{
    public class ReservationsCollectionDto
    {
        public IEnumerable<ReservationDto> Reservations { get; set; }
        public PaginationMetadataDto Pagination { get; set; }
    }
}