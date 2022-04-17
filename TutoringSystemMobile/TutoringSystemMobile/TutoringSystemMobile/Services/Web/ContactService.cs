using Flurl.Http;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Models.Dtos.Contact;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Web;
using Xamarin.Forms;

[assembly: Dependency(typeof(ContactService))]
namespace TutoringSystemMobile.Services.Web
{
    public class ContactService : IContactService
    {
        private readonly string baseUrl;

        public ContactService()
        {
            baseUrl = Settings.BaseApiUrl + ServicesConstans.Contact;
        }

        public async Task<ContactDetailsDto> GetContactByIdAsync(long contactId)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegment(contactId)
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<ContactDetailsDto>() : new ContactDetailsDto();
        }

        public async Task<ContactDetailsDto> GetContactByLoggedInUserAsync()
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<ContactDetailsDto>() : new ContactDetailsDto();
        }

        public async Task<bool> UpdateContactAsync(UpdatedContactDto updatedContact)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .PutJsonAsync(updatedContact);

            return response.StatusCode == 204;
        }
    }
}