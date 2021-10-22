using TutoringSystemMobile.Models.ContactDtos;

namespace TutoringSystemMobile.Models.PhoneNumberDtos
{
    public class PhoneNumberDetailsDto
    {
        public string Owner { get; set; }
        public string Number { get; set; }

        public ContactDetailsDto Contact { get; set; }
    }
}
