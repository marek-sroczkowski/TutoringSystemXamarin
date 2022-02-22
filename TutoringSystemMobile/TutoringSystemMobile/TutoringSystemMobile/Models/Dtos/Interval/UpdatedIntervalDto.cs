using System;

namespace TutoringSystemMobile.Models.Dtos.Interval
{
    public class UpdatedIntervalDto
    {
        public long Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
