namespace Aries.Contabilidad.Models.Auth
{
    public class LoginRequest
    {
        public string UserId { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
} 