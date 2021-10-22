using System.Collections.Generic;
using TutoringSystemMobile.Models.PhoneNumberDtos;

namespace TutoringSystemMobile.Models.ContactDtos
{
    public class ContactDto
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string DiscordName { get; set; }

        public ICollection<PhoneNumberDto> PhoneNumbers { get; set; }
    }
}
