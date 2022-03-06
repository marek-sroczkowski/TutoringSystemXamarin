using System.Threading.Tasks;
using TutoringSystemMobile.Models.Dtos.Contact;

namespace TutoringSystemMobile.Services.Interfaces
{
    public interface IContactService
    {
        Task<ContactDetailsDto> GetContactByLoggedInUserAsync();
        Task<ContactDetailsDto> GetContactByIdAsync(long contactId);
        Task<bool> UpdateContactAsync(UpdatedContactDto updatedContact);
    }
}
