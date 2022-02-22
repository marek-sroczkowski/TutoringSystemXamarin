using System.Collections.Generic;
using TutoringSystemMobile.Models.Dtos.PhoneNumber;

namespace TutoringSystemMobile.Models.Dtos.Contact
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
