namespace TutoringSystemMobile.Models.Dtos.Address
{
    public class UpdatedAddressDto
    {
        public long Id { get; set; }
        public string Street { get; set; }
        public string HouseAndFlatNumber { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Description { get; set; }

        public UpdatedAddressDto()
        {
        }

        public UpdatedAddressDto(long id, string street, string houseAndFlatNumber, string city, string postalCode, string description)
        {
            Id = id;
            Street = street;
            HouseAndFlatNumber = houseAndFlatNumber;
            City = city;
            PostalCode = postalCode;
            Description = description;
        }
    }
}