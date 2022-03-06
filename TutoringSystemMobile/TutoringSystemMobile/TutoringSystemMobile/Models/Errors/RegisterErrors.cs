using System.Collections.Generic;

namespace TutoringSystemMobile.Models.Errors
{
    public class RegisterErrors
    {
        public IEnumerable<string> Email { get; set; }
        public IEnumerable<string> Password { get; set; }
        public IEnumerable<string> Username { get; set; }
    }
}