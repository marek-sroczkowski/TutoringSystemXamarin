using System.Collections.Generic;
using TutoringSystemMobile.Models.PhoneNumberDtos;

namespace TutoringSystemMobile.Models.ContactDtos
{
    public class UpdatedContactDto
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string DiscordName { get; set; }

        public ICollection<UpdatedPhoneNumberDto> PhoneNumbers { get; set; }
    }
}
