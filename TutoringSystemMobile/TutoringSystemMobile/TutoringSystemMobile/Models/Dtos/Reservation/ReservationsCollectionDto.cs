using System.Collections.Generic;
using TutoringSystemMobile.Models.Pagination;

namespace TutoringSystemMobile.Models.Dtos.Reservation
{
    public class ReservationsCollectionDto
    {
        public IEnumerable<ReservationDto> Reservations { get; set; }
        public PaginationMetadata Pagination { get; set; }
    }
}