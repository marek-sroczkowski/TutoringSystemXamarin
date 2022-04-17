using Flurl.Http;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Models.Dtos.Address;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Web;
using Xamarin.Forms;

[assembly: Dependency(typeof(AddressService))]
namespace TutoringSystemMobile.Services.Web
{
    public class AddressService : IAddressService
    {
        private readonly string baseUrl;

        public AddressService()
        {
            baseUrl = Settings.BaseApiUrl + ServicesConstans.Address;
        }

        public async Task<AddressDetailsDto> GetAddressByIdAsync(long addressId)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegment(addressId)
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<AddressDetailsDto>() : new AddressDetailsDto();
        }

        public async Task<AddressDetailsDto> GetAddressOfLoggedInUserAsync()
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<AddressDetailsDto>() : new AddressDetailsDto();
        }

        public async Task<bool> UpdateAddressAsync(UpdatedAddressDto updatedAddress)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .PutJsonAsync(updatedAddress);

            return response.StatusCode == 204;
        }
    }
}