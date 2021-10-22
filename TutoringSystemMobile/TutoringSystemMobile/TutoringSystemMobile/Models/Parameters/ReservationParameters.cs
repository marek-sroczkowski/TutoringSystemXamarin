using System;
using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Models.Parameters
{
    public class ReservationParameters : QueryStringParameters
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ReservationPlace? Place { get; set; }
    }
}
