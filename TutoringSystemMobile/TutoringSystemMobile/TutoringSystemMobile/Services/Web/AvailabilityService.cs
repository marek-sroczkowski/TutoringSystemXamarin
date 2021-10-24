using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.AvailabilityDtos;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Services.Interfaces;

namespace TutoringSystemMobile.Services.Web
{
    public class AvailabilityService : IAvailabilityService
    {
        public Task<AvailabilityDto> AddAvailabilityAsync(NewAvailabilityDto newAvailability)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAvailabilityAsync(long availabilityId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<AvailabilityDto>> GetAvailabilitiesByTutorAsync(AvailabilityParameters parameters)
        {
            throw new NotImplementedException();
        }

        public Task<AvailabilityDetailsDto> GetAvailabilityByIdAsync(long availabilityId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<AvailabilityDto>> GetFutureAvailabilitiesByTutorAsync(FutureAvailabilityParameters parameters)
        {
            throw new NotImplementedException();
        }

        public Task<AvailabilityDetailsDto> GetTodaysAvailabilityByTutorAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAvailabilityAsync(UpdatedAvailabilityDto updatedAvailability)
        {
            throw new NotImplementedException();
        }
    }
}
