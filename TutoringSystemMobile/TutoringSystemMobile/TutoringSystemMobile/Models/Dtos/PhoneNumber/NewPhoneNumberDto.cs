namespace TutoringSystemMobile.Models.Dtos.PhoneNumber
{
    public class NewPhoneNumberDto
    {
        public string Owner { get; set; }
        public string Number { get; set; }

        public NewPhoneNumberDto()
        {
        }

        public NewPhoneNumberDto(string owner, string number)
        {
            Owner = owner;
            Number = number;
        }
    }
}
