using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.StudentDtos;
using TutoringSystemMobile.Models.TutorDtos;
using TutoringSystemMobile.Services.Interfaces;

namespace TutoringSystemMobile.Services.Web
{
    public class TutorService : ITutorService
    {
        public Task<bool> AddStudentAsync(long studentId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<StudentDto>> GetStudentsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TutorDetailsDto> GetTutorAsync(long tutorId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAllStudentsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveStudentAsync(long studentId)
        {
            throw new NotImplementedException();
        }
    }
}
