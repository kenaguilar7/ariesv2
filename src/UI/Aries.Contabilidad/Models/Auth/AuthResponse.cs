namespace Aries.Contabilidad.Models.Auth
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public UserInfo User { get; set; } = new();
    }
} 