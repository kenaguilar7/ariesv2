using System.Net.Http.Json;
using Aries.Contabilidad.Models.Accounts;

namespace Aries.Contabilidad.Services
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient _httpClient;

        public AccountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Account>> GetAccountsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<Account>>("Account/GetAccounts");
                return response ?? new List<Account>();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
} 