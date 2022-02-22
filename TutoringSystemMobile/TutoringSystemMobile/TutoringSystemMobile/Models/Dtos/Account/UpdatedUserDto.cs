namespace TutoringSystemMobile.Models.Dtos.Account
{
    public class UpdatedUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public UpdatedUserDto()
        {
        }

        public UpdatedUserDto(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}