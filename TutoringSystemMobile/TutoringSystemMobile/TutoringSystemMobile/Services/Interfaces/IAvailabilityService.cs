using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.AvailabilityDtos;
using TutoringSystemMobile.Models.Parameters;

namespace TutoringSystemMobile.Services.Interfaces
{
    public interface IAvailabilityService
    {
        Task<AvailabilityDto> AddAvailabilityAsync(NewAvailabilityDto newAvailability);
        Task<AvailabilityDetailsDto> GetAvailabilityByIdAsync(long availabilityId);
        Task<AvailabilityDetailsDto> GetTodaysAvailabilityByTutorAsync();
        Task<ICollection<AvailabilityDto>> GetAvailabilitiesByTutorAsync(AvailabilityParameters parameters);
        Task<ICollection<AvailabilityDto>> GetFutureAvailabilitiesByTutorAsync(FutureAvailabilityParameters parameters);
        Task<bool> UpdateAvailabilityAsync(UpdatedAvailabilityDto updatedAvailability);
        Task<bool> DeleteAvailabilityAsync(long availabilityId);
    }
}
