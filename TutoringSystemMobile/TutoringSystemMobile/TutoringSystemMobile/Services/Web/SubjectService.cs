using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.SubjectDtos;
using TutoringSystemMobile.Services.Interfaces;

namespace TutoringSystemMobile.Services.Web
{
    public class SubjectService : ISubjectService
    {
        public Task<SubjectDto> AddSubjectAsync(NewSubjectDto newSubjectModel)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteSubjectAsync(long subjectId)
        {
            throw new NotImplementedException();
        }

        public Task<SubjectDetailsDto> GetSubjectByIdAsync(long subjectId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<SubjectDto>> GetTutorSubjectsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateSubjectAsync(UpdatedSubjectDto updatedSubject)
        {
            throw new NotImplementedException();
        }
    }
}
