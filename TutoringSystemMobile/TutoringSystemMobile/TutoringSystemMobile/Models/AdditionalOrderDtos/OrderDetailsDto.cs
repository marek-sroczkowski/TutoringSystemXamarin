using System;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.TutorDtos;

namespace TutoringSystemMobile.Models.AdditionalOrderDtos
{
    public class OrderDetailsDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime ReceiptDate { get; set; }
        public DateTime Deadline { get; set; }
        public string Description { get; set; }
        public double Cost { get; set; }
        public bool IsPaid { get; set; }
        public AdditionalOrderStatus Status { get; set; }
        public TutorDto Tutor { get; set; }
    }
}
