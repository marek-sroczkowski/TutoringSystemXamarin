namespace TutoringSystemMobile.Models.Dtos.PhoneNumber
{
    public class UpdatedPhoneNumberDto
    {
        public long Id { get; set; }
        public string Owner { get; set; }
        public string Number { get; set; }

        public UpdatedPhoneNumberDto()
        {
        }

        public UpdatedPhoneNumberDto(long id, string owner, string number)
        {
            Id = id;
            Owner = owner;
            Number = number;
        }
    }
}
