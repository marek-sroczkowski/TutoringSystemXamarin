using TutoringSystemMobile.Models.Dtos.Address;
using TutoringSystemMobile.Models.Dtos.Contact;

namespace TutoringSystemMobile.Models.Dtos.Tutor
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
