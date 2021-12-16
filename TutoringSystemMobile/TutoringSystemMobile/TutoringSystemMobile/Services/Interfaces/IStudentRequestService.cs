using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.StudentRequestDtos;

namespace TutoringSystemMobile.Services.Interfaces
{
    public interface IStudentRequestService
    {
        Task<AddTutorToStudentStatus> AddRequestAsync(long tutorId);
        Task<bool> DeclineRequest(long requestId);
        Task<IEnumerable<StudentRequestDto>> GetRequestsByStudentId();
        Task<IEnumerable<StudentRequestDto>> GetRequestsByTutorId();
    }
}