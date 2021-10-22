using System;
using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Models.Parameters
{
    public class AdditionalOrderParameters : QueryStringParameters
    {
        public bool? IsPaid { get; set; }
        public AdditionalOrderStatus? Status { get; set; }
        public DateTime? ReceiptStartDate { get; set; }
        public DateTime? ReceiptEndDate { get; set; }
        public DateTime? DeadlineStart { get; set; }
        public DateTime? DeadlineEnd { get; set; }
    }
}
