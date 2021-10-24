using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.AccountDtos;
using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Services.Interfaces
{
    public interface IUserService
    {
        Task<LoginStatus> TryLoginAsync(LoginUserDto userModel);
        Task<bool> RegisterStudentAsync(RegisterStudentDto student);
        Task<bool> RegisterTutorAsync(RegisterTutorDto tutor);
        Task<bool> DeactivateUserAsync();
        Task<ICollection<WrongPasswordStatus>> ChangePasswordAsync(PasswordDto passwordModel);
        Task<bool> ActivateUserByTokenAsync(string activationToken);
        Task<bool> SendNewActivationTokenAsync();
    }
}
