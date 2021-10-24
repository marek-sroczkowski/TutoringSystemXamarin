using System.Threading.Tasks;
using TutoringSystemMobile.Models.AddressDtos;

namespace TutoringSystemMobile.Services.Interfaces
{
    public interface IAddressService
    {
        Task<AddressDetailsDto> GetAddressByUserAsync();
        Task<bool> UpdateAddressAsync(UpdatedAddressDto updatedAddress);
    }
}
