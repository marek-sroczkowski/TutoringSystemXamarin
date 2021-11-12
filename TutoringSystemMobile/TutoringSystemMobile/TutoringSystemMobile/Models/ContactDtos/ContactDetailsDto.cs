using System.Collections.Generic;
using TutoringSystemMobile.Models.PhoneNumberDtos;

namespace TutoringSystemMobile.Models.ContactDtos
{
    public class ContactDetailsDto
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string DiscordName { get; set; }
        public string Owner { get; set; }

        public IEnumerable<PhoneNumberDto> PhoneNumbers { get; set; }
    }
}
