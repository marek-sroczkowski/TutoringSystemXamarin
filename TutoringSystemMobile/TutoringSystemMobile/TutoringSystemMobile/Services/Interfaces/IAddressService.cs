using System.Threading.Tasks;
using TutoringSystemMobile.Models.Dtos.Address;

namespace TutoringSystemMobile.Services.Interfaces
{
    public interface IAddressService
    {
        Task<AddressDetailsDto> GetAddressOfLoggedInUserAsync();
        Task<AddressDetailsDto> GetAddressByIdAsync(long addressId);
        Task<bool> UpdateAddressAsync(UpdatedAddressDto updatedAddress);
    }
}
