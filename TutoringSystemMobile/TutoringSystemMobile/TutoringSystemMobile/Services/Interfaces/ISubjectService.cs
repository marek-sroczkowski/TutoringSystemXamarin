using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.Dtos.Subject;

namespace TutoringSystemMobile.Services.Interfaces
{
    public interface ISubjectService
    {
        Task<long> AddSubjectAsync(NewSubjectDto newSubjectModel);
        Task<bool> RemoveSubjectAsync(long subjectId);
        Task<SubjectDetailsDto> GetSubjectByIdAsync(long subjectId);
        Task<IEnumerable<SubjectDto>> GetSubjectByTutorId(long tutorId);
        Task<IEnumerable<SubjectDto>> GetSubjectsAsync();
        Task<long> UpdateSubjectAsync(UpdatedSubjectDto updatedSubject);
    }
}