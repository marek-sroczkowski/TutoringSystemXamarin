using System.Collections.Generic;
using TutoringSystemMobile.Models.Pagination;

namespace TutoringSystemMobile.Models.AdditionalOrderDtos
{
    public class OrdersCollectionDto
    {
        public IEnumerable<OrderDto> Orders { get; set; }
        public PaginationMetadataDto Pagination { get; set; }
    }
}