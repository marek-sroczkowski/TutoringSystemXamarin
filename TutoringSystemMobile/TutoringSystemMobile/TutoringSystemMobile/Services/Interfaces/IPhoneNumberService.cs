using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.PhoneNumberDtos;

namespace TutoringSystemMobile.Services.Interfaces
{
    public interface IPhoneNumberService
    {
        Task<bool> AddPhoneNumbersAsync(ICollection<NewPhoneNumberDto> phoneNumbers);
        Task<bool> DeletePhoneNumberAsync(long phoneNumberId);
        Task<PhoneNumberDetailsDto> GetPhoneNumberById(long phoneNumberId);
        Task<ICollection<PhoneNumberDto>> GetPhoneNumbersByUserAsync();
        Task<bool> UpdatePhoneNumberAsync(UpdatedPhoneNumberDto updatedPhoneNumber);
    }
}
