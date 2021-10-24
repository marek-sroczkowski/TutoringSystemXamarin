using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.PhoneNumberDtos;
using TutoringSystemMobile.Services.Interfaces;

namespace TutoringSystemMobile.Services.Web
{
    public class PhoneNumberService : IPhoneNumberService
    {
        public Task<bool> AddPhoneNumbersAsync(ICollection<NewPhoneNumberDto> phoneNumbers)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeletePhoneNumberAsync(long phoneNumberId)
        {
            throw new NotImplementedException();
        }

        public Task<PhoneNumberDetailsDto> GetPhoneNumberById(long phoneNumberId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<PhoneNumberDto>> GetPhoneNumbersByUserAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatePhoneNumberAsync(UpdatedPhoneNumberDto updatedPhoneNumber)
        {
            throw new NotImplementedException();
        }
    }
}
