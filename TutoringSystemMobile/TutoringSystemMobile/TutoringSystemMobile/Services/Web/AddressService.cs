using Flurl.Http;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.AddressDtos;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Web;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(AddressService))]
namespace TutoringSystemMobile.Services.Web
{
    public class AddressService : IAddressService
    {
        private readonly string baseUrl;

        public AddressService()
        {
            baseUrl = AppSettingsManager.Settings["BaseApiUrl"] + "address";
        }

        public async Task<AddressDetailsDto> GetAddressByIdAsync(long addressId)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .AppendPathSegment(addressId)
                .WithOAuthBearerToken(token)
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<AddressDetailsDto>() : new AddressDetailsDto();
        }

        public async Task<AddressDetailsDto> GetAddressOfLoggedInUserAsync()
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .WithOAuthBearerToken(token)
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<AddressDetailsDto>() : new AddressDetailsDto();
        }

        public async Task<bool> UpdateAddressAsync(UpdatedAddressDto updatedAddress)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .WithOAuthBearerToken(token)
                .PutJsonAsync(updatedAddress);

            return response.StatusCode == 204;
        }
    }
}