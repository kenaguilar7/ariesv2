using System.Net.Http.Json;
using System.Text.Json;
using Aries.Contabilidad.Models.Auth;
using Microsoft.JSInterop;

namespace Aries.Contabilidad.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        private const string TOKEN_KEY = "auth_token";
        private const string USER_KEY = "user_data";

        public AuthService(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
        }

        public async Task<AuthResponse?> LoginAsync(LoginRequest loginModel)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("auth/login", loginModel);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
                    if (result != null)
                    {
                        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TOKEN_KEY, result.Token);
                        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", USER_KEY, JsonSerializer.Serialize(result.User));
                        
                        // Set the authorization header for future requests
                        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.Token);
                        
                        return result;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task LogoutAsync()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TOKEN_KEY);
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", USER_KEY);
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            try
            {
                var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", TOKEN_KEY);
                return !string.IsNullOrEmpty(token);
            }
            catch
            {
                return false;
            }
        }
    }
} 