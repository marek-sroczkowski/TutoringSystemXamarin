using System.Collections.Generic;
using TutoringSystemMobile.Models.IntervalDtos;

namespace TutoringSystemMobile.Models.AvailabilityDtos
{
    public class UpdatedAvailabilityDto
    {
        public long Id { get; set; }
        public int BreakTime { get; set; }

        public ICollection<UpdatedIntervalDto> Intervals { get; set; }
    }
}
