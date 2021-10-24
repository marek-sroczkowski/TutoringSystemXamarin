using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.ContactDtos;
using TutoringSystemMobile.Services.Interfaces;

namespace TutoringSystemMobile.Services.Web
{
    public class ContactService : IContactService
    {
        public Task<ContactDetailsDto> GetContactByUserAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateContactAsync(UpdatedContactDto updatedContact)
        {
            throw new NotImplementedException();
        }
    }
}
