using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.StudentDtos;

namespace TutoringSystemMobile.Services.Interfaces
{
    public interface IStudentService
    {
        Task<AddStudentToTutorStatus> AddStudentToTutorAsync(NewExistingStudentDto student);
        Task<StudentDetailsDto> GetStudentByIdAsync(long studentId);
        Task<IEnumerable<StudentDto>> GetStudentsAsync();
        Task<bool> RemoveAllStudentsAsync();
        Task<bool> RemoveStudentAsync(long studentId);
    }
}
