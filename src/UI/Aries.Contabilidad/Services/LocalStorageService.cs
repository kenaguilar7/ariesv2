using System.Text.Json;
using Aries.Contabilidad.Models.Auth;
using Aries.Contabilidad.Models.DTOs;
using Aries.Contabilidad.Models.Enums;
using Microsoft.JSInterop;

namespace Aries.Contabilidad.Services
{
    public interface ILocalStorageService
    {
        Task StoreCompanyInLocalStorage(CompanyDto company);
        Task<CompanyDto?> GetStoredCompany();
        Task<string?> GetItem(string key);
        Task SetItem(string key, string value);
        Task RemoveItem(string key);
        Task StoreCurrentUser(UserInfo user);
        Task<UserInfo> GetCurrentUserSesion();
        Task<string?> GetAuthToken();
        Task StoreAuthToken(string token);
        Task RemoveAuthToken();
        Task RemoveCurrentUser();
    }

    public class LocalStorageService : ILocalStorageService
    {
        private readonly IJSRuntime _jsRuntime;
        public const string COMPANY_DATA_KEY = "company_data";
        public const string CURRENT_USER_KEY = "current_user_data";
        public const string AUTH_TOKEN_KEY = "auth_token";
        private readonly JsonSerializerOptions _jsonOptions;

        public LocalStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
        }

        public async Task StoreCompanyInLocalStorage(CompanyDto company)
        {
            try
            {
                Console.WriteLine($"Attempting to store company with code: {company.Code}");
                
                // Store the entire CompanyDto directly
                var jsonData = JsonSerializer.Serialize(company, _jsonOptions);
                Console.WriteLine($"Serialized company data: {jsonData}");
                await SetItem(COMPANY_DATA_KEY, jsonData);
                
                // Verify storage
                var verifyData = await GetItem(COMPANY_DATA_KEY);
                Console.WriteLine($"Verified stored data: {verifyData}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error storing company data in localStorage: {ex}");
                throw;
            }
        }

        public async Task<CompanyDto?> GetStoredCompany()
        {
            try
            {
                var companyDataJson = await GetItem(COMPANY_DATA_KEY);
                Console.WriteLine($"Retrieved company data from storage: {companyDataJson}");
                
                if (string.IsNullOrEmpty(companyDataJson))
                {
                    Console.WriteLine("No company data found in storage");
                    return null;
                }

                var company = JsonSerializer.Deserialize<CompanyDto>(companyDataJson, _jsonOptions);
                if (company == null)
                {
                    Console.WriteLine("Could not deserialize company data");
                    return null;
                }

                Console.WriteLine($"Successfully retrieved company with code: {company.Code}");
                return company;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving company data from localStorage: {ex}");
                return null;
            }
        }

        public async Task<string?> GetItem(string key)
        {
            try
            {
                return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting item from localStorage: {ex}");
                return null;
            }
        }

        public async Task SetItem(string key, string value)
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting item in localStorage: {ex}");
                throw;
            }
        }

        public async Task RemoveItem(string key)
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing item from localStorage: {ex}");
                throw;
            }
        }

        public async Task StoreCurrentUser(UserInfo user)
        {
            try
            {
                await SetItem(CURRENT_USER_KEY, JsonSerializer.Serialize(user, _jsonOptions));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error storing user data in localStorage: {ex}");
                throw;
            }
        }

        public async Task<UserInfo> GetCurrentUserSesion()
        {
            try
            {
                var userJson = await GetItem(CURRENT_USER_KEY);
                if (string.IsNullOrEmpty(userJson))
                    return new UserInfo();
                return JsonSerializer.Deserialize<UserInfo>(userJson, _jsonOptions) ?? new UserInfo();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user info from localStorage: {ex}");
                return new UserInfo();
            }
        }

        public async Task<string?> GetAuthToken()
        {
            return await GetItem(AUTH_TOKEN_KEY);
        }

        public async Task StoreAuthToken(string token)
        {
            await SetItem(AUTH_TOKEN_KEY, token);
        }

        public async Task RemoveAuthToken()
        {
            await RemoveItem(AUTH_TOKEN_KEY);
        }

        public async Task RemoveCurrentUser()
        {
            await RemoveItem(CURRENT_USER_KEY);
        }
    }
} 