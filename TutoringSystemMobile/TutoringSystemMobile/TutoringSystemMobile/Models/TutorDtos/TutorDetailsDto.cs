using TutoringSystemMobile.Models.AddressDtos;
using TutoringSystemMobile.Models.ContactDtos;

namespace TutoringSystemMobile.Models.TutorDtos
{
    public class TutorDetailsDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double HourlRate { get; set; }

        public ContactDto Contact { get; set; }
        public AddressDto Address { get; set; }
    }
}
