using System;
using System.Collections.Generic;
using TutoringSystemMobile.Models.IntervalDtos;

namespace TutoringSystemMobile.Models.AvailabilityDtos
{
    public class AvailabilityDto
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public int BreakTime { get; set; }

        public ICollection<IntervalDto> Intervals { get; set; }
    }
}
