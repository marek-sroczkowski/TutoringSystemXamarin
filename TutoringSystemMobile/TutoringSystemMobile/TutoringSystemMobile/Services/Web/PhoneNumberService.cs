using Flurl.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.Dtos.PhoneNumber;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Web;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(PhoneNumberService))]
namespace TutoringSystemMobile.Services.Web
{
    public class PhoneNumberService : IPhoneNumberService
    {
        private readonly string baseUrl;

        public PhoneNumberService()
        {
            baseUrl = AppSettingsManager.Settings["BaseApiUrl"] + "contact";
        }

        public async Task<bool> AddPhoneNumberAsync(long contactId, NewPhoneNumberDto newPhoneNumber)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .AppendPathSegments(contactId, "phoneNumber")
                .WithOAuthBearerToken(token)
                .PostJsonAsync(newPhoneNumber);

            return response.StatusCode == 201;
        }

        public async Task<bool> DeletePhoneNumberAsync(long contactId, long phoneNumberId)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .AppendPathSegments(contactId, "phoneNumber", phoneNumberId)
                .WithOAuthBearerToken(token)
                .DeleteAsync();

            return response.StatusCode == 204;
        }

        public async Task<PhoneNumberDetailsDto> GetPhoneNumberById(long contactId, long phoneNumberId)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .AppendPathSegments(contactId, "phoneNumber", phoneNumberId)
                .WithOAuthBearerToken(token)
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<PhoneNumberDetailsDto>() : new PhoneNumberDetailsDto();
        }

        public async Task<IEnumerable<PhoneNumberDto>> GetPhoneNumbersByContactIdAsync(long contactId)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .AppendPathSegments(contactId, "phoneNumber")
                .WithOAuthBearerToken(token)
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<IEnumerable<PhoneNumberDto>>() : new List<PhoneNumberDto>();
        }

        public async Task<bool> UpdatePhoneNumberAsync(long contactId, UpdatedPhoneNumberDto updatedPhoneNumber)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .AppendPathSegments(contactId, "phoneNumber")
                .WithOAuthBearerToken(token)
                .PutJsonAsync(updatedPhoneNumber);

            return response.StatusCode == 204;
        }
    }
}