using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.AddressDtos;
using TutoringSystemMobile.Services.Interfaces;

namespace TutoringSystemMobile.Services.Web
{
    public class AddressService : IAddressService
    {
        public Task<AddressDetailsDto> GetAddressByUserAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAddressAsync(UpdatedAddressDto updatedAddress)
        {
            throw new NotImplementedException();
        }
    }
}
