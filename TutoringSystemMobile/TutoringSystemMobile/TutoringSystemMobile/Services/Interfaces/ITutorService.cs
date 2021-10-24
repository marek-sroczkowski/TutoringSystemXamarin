using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.StudentDtos;
using TutoringSystemMobile.Models.TutorDtos;

namespace TutoringSystemMobile.Services.Interfaces
{
    public interface ITutorService
    {
        Task<bool> AddStudentAsync(long studentId);
        Task<ICollection<StudentDto>> GetStudentsAsync();
        Task<TutorDetailsDto> GetTutorAsync(long tutorId);
        Task<bool> RemoveAllStudentsAsync();
        Task<bool> RemoveStudentAsync(long studentId);
    }
}
