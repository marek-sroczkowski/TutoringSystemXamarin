using System;
using System.Collections.Generic;
using TutoringSystemMobile.Models.IntervalDtos;

namespace TutoringSystemMobile.Models.AvailabilityDtos
{
    public class NewAvailabilityDto
    {
        public DateTime Date { get; set; }
        public int BreakTime { get; set; }

        public ICollection<NewIntervalDto> Intervals { get; set; }
    }
}
