using Flurl.Http;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.Dtos.Contact;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Web;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(ContactService))]
namespace TutoringSystemMobile.Services.Web
{
    public class ContactService : IContactService
    {
        private readonly string baseUrl;

        public ContactService()
        {
            baseUrl = AppSettingsManager.Settings["BaseApiUrl"] + "contact";
        }

        public async Task<ContactDetailsDto> GetContactByIdAsync(long contactId)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .AppendPathSegment(contactId)
                .WithOAuthBearerToken(token)
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<ContactDetailsDto>() : new ContactDetailsDto();
        }

        public async Task<ContactDetailsDto> GetContactByLoggedInUserAsync()
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .WithOAuthBearerToken(token)
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<ContactDetailsDto>() : new ContactDetailsDto();
        }

        public async Task<bool> UpdateContactAsync(UpdatedContactDto updatedContact)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .WithOAuthBearerToken(token)
                .PutJsonAsync(updatedContact);

            return response.StatusCode == 204;
        }
    }
}