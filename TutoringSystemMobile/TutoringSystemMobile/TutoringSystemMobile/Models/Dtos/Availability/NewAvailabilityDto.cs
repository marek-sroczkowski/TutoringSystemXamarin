using System;
using System.Collections.Generic;
using TutoringSystemMobile.Models.Dtos.Interval;

namespace TutoringSystemMobile.Models.Dtos.Availability
{
    public class NewAvailabilityDto
    {
        public DateTime Date { get; set; }
        public int BreakTime { get; set; }

        public ICollection<NewIntervalDto> Intervals { get; set; }
    }
}
