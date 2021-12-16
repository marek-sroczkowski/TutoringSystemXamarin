using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Models.TutorDtos;

namespace TutoringSystemMobile.Services.Interfaces
{
    public interface ITutorService
    {
        Task<IEnumerable<TutorDto>> GetTutorsByStudentAsync();
        Task<TutorDetailsDto> GetTutorByIdAsync(long tutorId);
        Task<bool> RemoveAllTutorsAsync();
        Task<bool> RemoveTutorAsync(long tutorId);
        Task<TutorsCollectionDto> GetTutorsByParamsAsync(SearchedUserParameters parameters);
    }
}
