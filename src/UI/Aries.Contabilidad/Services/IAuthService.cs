using Aries.Contabilidad.Models.Auth;

namespace Aries.Contabilidad.Services
{
    public interface IAuthService
    {
        Task<AuthResponse?> LoginAsync(LoginRequest loginModel);
        Task LogoutAsync();
        Task<bool> IsAuthenticatedAsync();
    }
} 