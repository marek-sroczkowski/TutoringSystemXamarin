using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.AccountDtos;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.Errors;

namespace TutoringSystemMobile.Services.Interfaces
{
    public interface IUserService
    {
        Task<LoginStatus> TryLoginAsync(LoginUserDto userModel);
        Task<RegisterErrorTypes> RegisterStudentAsync(RegisterStudentDto student);
        Task<RegisterErrorTypes> RegisterTutorAsync(RegisterTutorDto tutor);
        Task<bool> DeactivateUserAsync();
        Task<ICollection<WrongPasswordStatus>> ChangePasswordAsync(PasswordDto passwordModel);
        Task<bool> ActivateUserByTokenAsync(string activationToken);
        Task<bool> SendNewActivationTokenAsync();
        Task<Role> GetUserRole();
    }
}
