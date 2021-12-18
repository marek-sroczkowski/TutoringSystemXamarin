using System;

namespace TutoringSystemMobile.Models.Parameters
{
    public class ReservationParameters : QueryStringParameters
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsAtTutor { get; set; }
        public bool IsAtStudent { get; set; }
        public bool IsOnline { get; set; }
    }
}