using System.Text.Json;
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
    }

    public class LocalStorageService : ILocalStorageService
    {
        private readonly IJSRuntime _jsRuntime;
        private const string USER_DATA_KEY = "user_data";

        public LocalStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task StoreCompanyInLocalStorage(CompanyDto company)
        {
            try
            {
                var userData = new
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
                await SetItem(USER_DATA_KEY, JsonSerializer.Serialize(userData));
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
                var userDataJson = await GetItem(USER_DATA_KEY);
                if (string.IsNullOrEmpty(userDataJson))
                    return null;

                var userData = JsonSerializer.Deserialize<UserData>(userDataJson);
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