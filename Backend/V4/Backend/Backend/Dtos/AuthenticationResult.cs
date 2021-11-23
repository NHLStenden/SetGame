using System;

namespace Backend.Dtos
{
    public class AuthenticationResult
    {
        public string[] Errors { get; set; }
        public bool Success { get; set; }
        public string Token { get; set; }
        public Guid RefreshToken { get; set; }
    }
}