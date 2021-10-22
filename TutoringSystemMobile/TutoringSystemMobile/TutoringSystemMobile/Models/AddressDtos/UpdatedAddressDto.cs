namespace TutoringSystemMobile.Models.AddressDtos
{
    public class UpdatedAddressDto
    {
        public long Id { get; set; }
        public string Street { get; set; }
        public string HouseAndFlatNumber { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Description { get; set; }
    }
}
