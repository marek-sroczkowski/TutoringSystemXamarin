using System;
using System.Collections.Generic;
using TutoringSystemMobile.Models.Dtos.Interval;

namespace TutoringSystemMobile.Models.Dtos.Availability
{
    public class AvailabilityDto
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public int BreakTime { get; set; }

        public ICollection<IntervalDto> Intervals { get; set; }
    }
}
