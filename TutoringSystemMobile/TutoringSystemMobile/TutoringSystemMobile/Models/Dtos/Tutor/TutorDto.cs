namespace TutoringSystemMobile.Models.Dtos.Tutor
{
    public class TutorDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double HourlRate { get; set; }
        public string ProfilePictureBase64 { get; set; }
    }
}
