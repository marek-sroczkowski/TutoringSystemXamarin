namespace TutoringSystemMobile.Models.Errors
{
    public class RegisterError
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public string TraceId { get; set; }
        public RegisterErrorTypes Errors { get; set; }
    }
}
