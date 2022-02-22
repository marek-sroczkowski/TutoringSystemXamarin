using System.Collections.Generic;
using TutoringSystemMobile.Models.Pagination;

namespace TutoringSystemMobile.Models.Dtos.AdditionalOrder
{
    public class OrdersCollectionDto
    {
        public IEnumerable<OrderDto> Orders { get; set; }
        public PaginationMetadata Pagination { get; set; }
    }
}