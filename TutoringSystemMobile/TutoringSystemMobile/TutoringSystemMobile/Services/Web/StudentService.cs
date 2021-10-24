using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.StudentDtos;
using TutoringSystemMobile.Models.TutorDtos;
using TutoringSystemMobile.Services.Interfaces;

namespace TutoringSystemMobile.Services.Web
{
    public class StudentService : IStudentService
    {
        public Task<bool> AddTutorAsync(long tutorId)
        {
            throw new NotImplementedException();
        }

        public Task<StudentDetailsDto> GetStudentAsync(long studentId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<TutorDto>> GetTutorsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAllTutorsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveTutorAsync(long tutorId)
        {
            throw new NotImplementedException();
        }
    }
}
