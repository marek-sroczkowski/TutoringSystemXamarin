using System.Threading.Tasks;
using TutoringSystemMobile.Models.Dtos.Account;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.Errors;

namespace TutoringSystemMobile.Services.Interfaces
{
    public interface IUserService
    {
        Task<RegisterErrors> RegisterStudentAsync(RegisteredStudentDto student);
        Task<RegisterErrors> RegisterTutorAsync(RegisteredTutorDto tutor);
        Task<bool> DeactivateUserAsync();
        Task<PasswordChangeError> ChangePasswordAsync(PasswordDto passwordModel);
        Task<bool> ActivateUserByTokenAsync(string activationToken);
        Task<bool> SendNewActivationTokenAsync();
        Task<Role> GetUserRole();
        Task<bool> UpdateGeneralUserInfoAsync(UpdatedUserDto updatedUser);
        Task<ShortUserDto> GetGeneralUserInfoAsync();
        Task<RegisterErrors> CreateStudentAsync(NewStudentDto student);
    }
}