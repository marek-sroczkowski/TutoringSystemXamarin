using Flurl;
using Flurl.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.ImagesDtos;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Web;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(ImageService))]
namespace TutoringSystemMobile.Services.Web
{
    public class ImageService : IImageService
    {
        private readonly string baseUrl;

        public ImageService()
        {
            baseUrl = AppSettingsManager.Settings["BaseApiUrl"] + "image";
        }

        public async Task<bool> SetProfileImageAsync(ProfileImageDto image)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .WithOAuthBearerToken(token)
                .PatchJsonAsync(image);

            return response.StatusCode == 204;
        }

        public async Task<bool> RemoveProfileImageAsync()
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .WithOAuthBearerToken(token)
                .DeleteAsync();

            return response.StatusCode == 204;
        }

        public async Task<ProfileImageDetailsDto> GetProfileImageAsync()
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .WithOAuthBearerToken(token)
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<ProfileImageDetailsDto>() : new ProfileImageDetailsDto();
        }

        public async Task<IEnumerable<ProfileImageDetailsDto>> GetStudentPhotos()
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AppendPathSegment("students")
                .AllowAnyHttpStatus()
                .WithOAuthBearerToken(token)
                .GetAsync();

            return response.StatusCode == 200 ?
                await response.GetJsonAsync<IEnumerable<ProfileImageDetailsDto>>() :
                new List<ProfileImageDetailsDto>();
        }

        public async Task<IEnumerable<ProfileImageDetailsDto>> GetTutorPhotos()
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AppendPathSegment("tutors")
                .AllowAnyHttpStatus()
                .WithOAuthBearerToken(token)
                .GetAsync();

            return response.StatusCode == 200 ?
                await response.GetJsonAsync<IEnumerable<ProfileImageDetailsDto>>() :
                new List<ProfileImageDetailsDto>();
        }
    }
}