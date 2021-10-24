using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.StudentDtos;
using TutoringSystemMobile.Models.TutorDtos;

namespace TutoringSystemMobile.Services.Interfaces
{
    public interface IStudentService
    {
        Task<bool> AddTutorAsync(long tutorId);
        Task<StudentDetailsDto> GetStudentAsync(long studentId);
        Task<ICollection<TutorDto>> GetTutorsAsync();
        Task<bool> RemoveAllTutorsAsync();
        Task<bool> RemoveTutorAsync(long tutorId);
    }
}
