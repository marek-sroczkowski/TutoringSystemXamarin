using TutoringSystemMobile.Models.AddressDtos;
using TutoringSystemMobile.Models.ContactDtos;

namespace TutoringSystemMobile.Models.StudentDtos
{
    public class StudentDetailsDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double HourlRate { get; set; }
        public string Note { get; set; }

        public ContactDto Contact { get; set; }
        public AddressDto Address { get; set; }
    }
}
