using System.Collections.Generic;

namespace TutoringSystemMobile.Models.Errors
{
    public class RefreshTokenError
    {
        public IEnumerable<string> Errors { get; set; }
    }
}
