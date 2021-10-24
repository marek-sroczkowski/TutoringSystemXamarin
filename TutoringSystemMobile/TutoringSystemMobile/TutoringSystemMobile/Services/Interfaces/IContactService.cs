using System.Threading.Tasks;
using TutoringSystemMobile.Models.ContactDtos;

namespace TutoringSystemMobile.Services.Interfaces
{
    public interface IContactService
    {
        Task<ContactDetailsDto> GetContactByUserAsync();
        Task<bool> UpdateContactAsync(UpdatedContactDto updatedContact);
    }
}
