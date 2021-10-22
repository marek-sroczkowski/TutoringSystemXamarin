using System;
using System.Collections.Generic;
using TutoringSystemMobile.Models.IntervalDtos;
using TutoringSystemMobile.Models.TutorDtos;

namespace TutoringSystemMobile.Models.AvailabilityDtos
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
