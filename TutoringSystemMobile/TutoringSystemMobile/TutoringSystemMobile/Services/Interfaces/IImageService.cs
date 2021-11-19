using System.Threading.Tasks;
using TutoringSystemMobile.Models.AccountDtos;

namespace TutoringSystemMobile.Services.Interfaces
{
    public interface IImageService
    {
        Task<ProfileImageDetailsDto> GetProfileImageAsync();
        Task<bool> RemoveProfileImageAsync();
        Task<bool> SetProfileImageAsync(ProfileImageDto image);
    }
}
