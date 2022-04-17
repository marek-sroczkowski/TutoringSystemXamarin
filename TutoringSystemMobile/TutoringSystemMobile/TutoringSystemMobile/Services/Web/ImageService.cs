using Flurl;
using Flurl.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Models.Dtos.Images;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Web;
using Xamarin.Forms;

[assembly: Dependency(typeof(ImageService))]
namespace TutoringSystemMobile.Services.Web
{
    public class ImageService : IImageService
    {
        private readonly string baseUrl;

        public ImageService()
        {
            baseUrl = Settings.BaseApiUrl + ServicesConstans.Image;
        }

        public async Task<bool> SetProfileImageAsync(ProfileImageDto image)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .PatchJsonAsync(image);

            return response.StatusCode == 204;
        }

        public async Task<bool> RemoveProfileImageAsync()
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .DeleteAsync();

            return response.StatusCode == 204;
        }

        public async Task<ProfileImageDetailsDto> GetProfileImageAsync()
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<ProfileImageDetailsDto>() : new ProfileImageDetailsDto();
        }

        public async Task<IEnumerable<ProfileImageDetailsDto>> GetStudentPhotos()
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegment(ServicesConstans.Students)
                .GetAsync();

            return response.StatusCode == 200
                ? await response.GetJsonAsync<IEnumerable<ProfileImageDetailsDto>>()
                : new List<ProfileImageDetailsDto>();
        }

        public async Task<IEnumerable<ProfileImageDetailsDto>> GetTutorPhotos()
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegment(ServicesConstans.Tutors)
                .GetAsync();

            return response.StatusCode == 200
                ? await response.GetJsonAsync<IEnumerable<ProfileImageDetailsDto>>()
                : new List<ProfileImageDetailsDto>();
        }
    }
}