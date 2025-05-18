using System.Security.Claims;
using System.Text.Json;
using Aries.Contabilidad.Models.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace Aries.Contabilidad.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly IAuthService _authService;
        private const string TOKEN_KEY = "auth_token";
        private const string USER_KEY = "user_data";

        public CustomAuthStateProvider(IJSRuntime jsRuntime, IAuthService authService)
        {
            _jsRuntime = jsRuntime;
            _authService = authService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", TOKEN_KEY);
                var userJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", USER_KEY);

                if (string.IsNullOrEmpty(token))
                {
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                var user = JsonSerializer.Deserialize<UserInfo>(userJson);
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.UserType.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                };

                var identity = new ClaimsIdentity(claims, "jwt");
                return new AuthenticationState(new ClaimsPrincipal(identity));
            }
            catch
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        public async Task MarkUserAsAuthenticated(AuthResponse authResponse)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, authResponse.User.UserName),
                new Claim(ClaimTypes.Role, authResponse.User.UserType.ToString()),
                new Claim(ClaimTypes.NameIdentifier, authResponse.User.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public async Task MarkUserAsLoggedOut()
        {
            await _authService.LogoutAsync();
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
    }
} 