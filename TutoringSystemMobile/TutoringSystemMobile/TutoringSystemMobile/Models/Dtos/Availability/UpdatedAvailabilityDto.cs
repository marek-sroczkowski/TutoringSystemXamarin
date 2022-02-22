using System.Collections.Generic;
using TutoringSystemMobile.Models.Dtos.Interval;

namespace TutoringSystemMobile.Models.Dtos.Availability
{
    public class UpdatedAvailabilityDto
    {
        public long Id { get; set; }
        public int BreakTime { get; set; }

        public ICollection<UpdatedIntervalDto> Intervals { get; set; }
    }
}
