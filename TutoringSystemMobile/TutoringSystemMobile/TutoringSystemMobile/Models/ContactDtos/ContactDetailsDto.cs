using System.Collections.Generic;
using TutoringSystemMobile.Models.AccountDtos;
using TutoringSystemMobile.Models.PhoneNumberDtos;

namespace TutoringSystemMobile.Models.ContactDtos
{
    public class ContactDetailsDto
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string DiscordName { get; set; }

        public ICollection<PhoneNumberDto> PhoneNumbers { get; set; }

        public UserDto User { get; set; }
    }
}
