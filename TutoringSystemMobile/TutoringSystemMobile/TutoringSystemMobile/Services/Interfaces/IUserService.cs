using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.Dtos.Account;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.Errors;

namespace TutoringSystemMobile.Services.Interfaces
{
    public interface IUserService
    {
        Task<LoginResposneDto> TryLoginAsync(LoginUserDto userModel);
        Task<RegisterErrors> RegisterStudentAsync(RegisterStudentDto student);
        Task<RegisterErrors> RegisterTutorAsync(RegisterTutorDto tutor);
        Task<bool> DeactivateUserAsync();
        Task<IEnumerable<WrongPasswordStatus>> ChangePasswordAsync(PasswordDto passwordModel);
        Task<bool> ActivateUserByTokenAsync(string activationToken);
        Task<bool> SendNewActivationTokenAsync();
        Task<Role> GetUserRole();
        Task<bool> UpdateGeneralUserInfoAsync(UpdatedUserDto updatedUser);
        Task<ShortUserDto> GetGeneralUserInfoAsync();
    }
}
