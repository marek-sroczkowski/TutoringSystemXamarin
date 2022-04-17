using System;

namespace TutoringSystemMobile.Models.Dtos.Authentication
{
    public class TokenDto
    {
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}