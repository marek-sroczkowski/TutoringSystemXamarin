using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.PhoneNumberDtos;

namespace TutoringSystemMobile.Services.Interfaces
{
    public interface IPhoneNumberService
    {
        Task<bool> AddPhoneNumberAsync(long contactId, NewPhoneNumberDto phoneNumber);
        Task<bool> DeletePhoneNumberAsync(long contactId, long phoneNumberId);
        Task<PhoneNumberDetailsDto> GetPhoneNumberById(long contactId, long phoneNumberId);
        Task<IEnumerable<PhoneNumberDto>> GetPhoneNumbersByContactIdAsync(long contactId);
        Task<bool> UpdatePhoneNumberAsync(long contactId, UpdatedPhoneNumberDto updatedPhoneNumber);
    }
}
