using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Models.Dtos.Student;

namespace TutoringSystemMobile.Services.Interfaces
{
    public interface IStudentService
    {
        Task<AddStudentToTutorStatus> AddStudentToTutorAsync(NewExistingStudentDto student);
        Task<StudentDetailsDto> GetStudentByIdAsync(long studentId);
        Task<IEnumerable<StudentDto>> GetStudentsAsync();
        Task<StudentsCollectionDto> GetStudentsByParamsAsync(SearchedUserParameters parameters);
        Task<bool> RemoveAllStudentsAsync();
        Task<bool> RemoveStudentAsync(long studentId);
        Task<bool> UpdateStudentAsync(UpdatedStudentDto student);
    }
}
