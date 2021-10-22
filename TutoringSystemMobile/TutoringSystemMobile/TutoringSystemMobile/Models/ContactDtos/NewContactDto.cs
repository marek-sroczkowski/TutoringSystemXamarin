using System.Collections.Generic;
using TutoringSystemMobile.Models.PhoneNumberDtos;

namespace TutoringSystemMobile.Models.ContactDtos
{
    public class NewContactDto
    {
        public string Email { get; set; }
        public string DiscordName { get; set; }

        public ICollection<NewPhoneNumberDto> PhoneNumbers { get; set; }
    }
}
