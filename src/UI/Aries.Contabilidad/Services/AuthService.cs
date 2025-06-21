using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Aries.Contabilidad.Models.Auth;
using Microsoft.JSInterop;

namespace Aries.Contabilidad.Services
{
    public class AuthService : BaseHttpService, IAuthService
    {
        private readonly ILocalStorageService _localStorageService;

        public AuthService(IHttpClientFactory httpClientFactory, ILocalStorageService localStorageService)
            : base(httpClientFactory)
        {
            _localStorageService = localStorageService;
        }

        public async Task<AuthResponse?> LoginAsync(LoginRequest loginModel)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Auth/login", loginModel);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
                    if (result != null)
                    {
                        await _localStorageService.StoreAuthToken(result.Token);
                        await _localStorageService.StoreCurrentUser(result.User);

                        // Set the authorization header for future requests
                        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.Token);
                        
                        return result;
                    }
                }
                return null;
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public async Task LogoutAsync()
        {
            await _localStorageService.RemoveAuthToken();
            await _localStorageService.RemoveCurrentUser();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            try
            {
                var token = await _localStorageService.GetAuthToken();
                return !string.IsNullOrEmpty(token);
            }
            catch
            {
                return false;
            }
        }
    }
} 