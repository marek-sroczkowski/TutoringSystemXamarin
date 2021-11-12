using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.TutorDtos;

namespace TutoringSystemMobile.Services.Interfaces
{
    public interface ITutorService
    {
        Task<IEnumerable<TutorDto>> GetTutorsAsync();
        Task<TutorDetailsDto> GetTutorByIdAsync(long tutorId);
        Task<bool> RemoveAllTutorsAsync();
        Task<bool> RemoveTutorAsync(long tutorId);
        Task<bool> AddTutorToStudentAsync(long tutorId);
    }
}
