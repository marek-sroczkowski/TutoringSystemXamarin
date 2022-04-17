using Flurl.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Models.Dtos.PhoneNumber;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Web;
using Xamarin.Forms;

[assembly: Dependency(typeof(PhoneNumberService))]
namespace TutoringSystemMobile.Services.Web
{
    public class PhoneNumberService : IPhoneNumberService
    {
        private readonly string baseUrl;

        public PhoneNumberService()
        {
            baseUrl = Settings.BaseApiUrl + ServicesConstans.Contact;
        }

        public async Task<bool> AddPhoneNumberAsync(long contactId, NewPhoneNumberDto newPhoneNumber)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegments(contactId, ServicesConstans.PhoneNumber)
                .PostJsonAsync(newPhoneNumber);

            return response.StatusCode == 201;
        }

        public async Task<bool> DeletePhoneNumberAsync(long contactId, long phoneNumberId)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegments(contactId, ServicesConstans.PhoneNumber, phoneNumberId)
                .DeleteAsync();

            return response.StatusCode == 204;
        }

        public async Task<PhoneNumberDetailsDto> GetPhoneNumberById(long contactId, long phoneNumberId)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegments(contactId, ServicesConstans.PhoneNumber, phoneNumberId)
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<PhoneNumberDetailsDto>() : new PhoneNumberDetailsDto();
        }

        public async Task<IEnumerable<PhoneNumberDto>> GetPhoneNumbersByContactIdAsync(long contactId)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegments(contactId, ServicesConstans.PhoneNumber)
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<IEnumerable<PhoneNumberDto>>() : new List<PhoneNumberDto>();
        }

        public async Task<bool> UpdatePhoneNumberAsync(long contactId, UpdatedPhoneNumberDto updatedPhoneNumber)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegments(contactId, ServicesConstans.PhoneNumber)
                .PutJsonAsync(updatedPhoneNumber);

            return response.StatusCode == 204;
        }
    }
}