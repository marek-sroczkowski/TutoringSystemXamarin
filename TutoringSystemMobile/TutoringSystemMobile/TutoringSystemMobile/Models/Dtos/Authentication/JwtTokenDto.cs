using System;

namespace TutoringSystemMobile.Models.Dtos.Authentication
{
    public class JwtTokenDto
    {
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}