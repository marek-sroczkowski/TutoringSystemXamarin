using System;

namespace TutoringSystemMobile.Models.IntervalDtos
{
    public class UpdatedIntervalDto
    {
        public long Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
