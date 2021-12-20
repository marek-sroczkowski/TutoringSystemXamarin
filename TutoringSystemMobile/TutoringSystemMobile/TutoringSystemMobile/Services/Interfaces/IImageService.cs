using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.ImagesDtos;

namespace TutoringSystemMobile.Services.Interfaces
{
    public interface IImageService
    {
        Task<ProfileImageDetailsDto> GetProfileImageAsync();
        Task<IEnumerable<ProfileImageDetailsDto>> GetStudentPhotos();
        Task<IEnumerable<ProfileImageDetailsDto>> GetTutorPhotos();
        Task<bool> RemoveProfileImageAsync();
        Task<bool> SetProfileImageAsync(ProfileImageDto image);
    }
}