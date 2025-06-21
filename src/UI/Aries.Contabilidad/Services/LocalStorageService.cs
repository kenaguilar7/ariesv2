using System.Text.Json;
using Aries.Contabilidad.Models.Auth;
using Aries.Contabilidad.Models.DTOs;
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

        public LocalStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task StoreCompanyInLocalStorage(CompanyDto company)
        {
            try
            {
                var companyData = new
                {
                    selectedCompany = new
                    {
                        id = company.Id,
                        code = company.Code,
                        name = company.CompanyName,
                        idType = company.IdType,
                        numberId = company.NumberId,
                        moneyType = company.MoneyType,
                        lastAccessed = DateTime.Now
                    }
                };
                await SetItem(COMPANY_DATA_KEY, JsonSerializer.Serialize(companyData));
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
                if (string.IsNullOrEmpty(companyDataJson))
                    return null;

                var userData = JsonSerializer.Deserialize<UserData>(companyDataJson);
                if (userData?.selectedCompany == null)
                    return null;

                return new CompanyDto
                {
                    Id = userData.selectedCompany.id,
                    Code = userData.selectedCompany.code,
                    CompanyName = userData.selectedCompany.name,
                    //IdType = userData.selectedCompany.idType,
                    //NumberId = userData.selectedCompany.numberId,
                    //MoneyType = userData.selectedCompany.moneyType
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving company data from localStorage: {ex}");
                return null;
            }
        }

        public async Task<UserInfo> GetCurrentUserSesion()
        {
            try
            {
                var userJson = await GetItem(CURRENT_USER_KEY);
                if (string.IsNullOrEmpty(userJson))
                    return new UserInfo();
                return JsonSerializer.Deserialize<UserInfo>(userJson) ?? new UserInfo();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user info from localStorage: {ex}");
                return new UserInfo();
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
                await SetItem(CURRENT_USER_KEY, JsonSerializer.Serialize(user));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error storing user data in localStorage: {ex}");
                throw;
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

        private class UserData
        {
            public SelectedCompany? selectedCompany { get; set; }
        }

        private class SelectedCompany
        {
            public int id { get; set; }
            public string code { get; set; } = string.Empty;
            public string name { get; set; } = string.Empty;
            public string idType { get; set; } = string.Empty;
            public string numberId { get; set; } = string.Empty;
            public string moneyType { get; set; } = string.Empty;
            public DateTime lastAccessed { get; set; }
        }
    }
} 