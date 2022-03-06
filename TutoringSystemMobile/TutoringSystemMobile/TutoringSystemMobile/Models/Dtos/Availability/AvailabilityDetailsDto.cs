using System;
using System.Collections.Generic;
using TutoringSystemMobile.Models.Dtos.Interval;
using TutoringSystemMobile.Models.Dtos.Tutor;

namespace TutoringSystemMobile.Models.Dtos.Availability
{
    public class AvailabilityDetailsDto
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public int BreakTime { get; set; }

        public ICollection<IntervalDto> Intervals { get; set; }
        public TutorDto Tutor { get; set; }
    }
}
