using TutoringSystemMobile.Models.AccountDtos;

namespace TutoringSystemMobile.Models.AddressDtos
{
    public class AddressDetailsDto
    {
        public long Id { get; set; }
        public string Street { get; set; }
        public string HouseAndFlatNumber { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Description { get; set; }

        public UserDto User { get; set; }
    }
}
