namespace TutoringSystemMobile.Models.Dtos.Contact
{
    public class UpdatedContactDto
    {
        public long Id { get; set; }
        public string DiscordName { get; set; }

        public UpdatedContactDto()
        {
        }

        public UpdatedContactDto(long id, string discordName)
        {
            Id = id;
            DiscordName = discordName;
        }
    }
}