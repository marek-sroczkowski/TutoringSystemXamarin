using System;
using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Models.AdditionalOrderDtos
{
    public class OrderDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime Deadline { get; set; }
        public double Cost { get; set; }
        public bool IsPaid { get; set; }
        public AdditionalOrderStatus Status { get; set; }
    }
}
