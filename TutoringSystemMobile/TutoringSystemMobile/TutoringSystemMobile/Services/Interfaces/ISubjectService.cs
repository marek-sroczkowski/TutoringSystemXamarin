using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.SubjectDtos;

namespace TutoringSystemMobile.Services.Interfaces
{
    public interface ISubjectService
    {
        Task<SubjectDto> AddSubjectAsync(NewSubjectDto newSubjectModel);
        Task<bool> DeleteSubjectAsync(long subjectId);
        Task<SubjectDetailsDto> GetSubjectByIdAsync(long subjectId);
        Task<ICollection<SubjectDto>> GetTutorSubjectsAsync();
        Task<bool> UpdateSubjectAsync(UpdatedSubjectDto updatedSubject);
    }
}
