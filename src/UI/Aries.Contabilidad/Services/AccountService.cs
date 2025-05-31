using System.Net.Http.Json;
using Aries.Contabilidad.Models.Accounts;
using Aries.Contabilidad.Models.DTOs;
using Microsoft.Extensions.Logging;

namespace Aries.Contabilidad.Services
{
    public class AccountService : BaseHttpService, IAccountService
    {
        public AccountService(IHttpClientFactory httpClientFactory, ILogger<AccountService> logger)
            : base(httpClientFactory, logger)
        {
        }
        public const string ServiceName = "Account";

        public async Task<List<Account>> GetAccountsAsync(string companyId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<Account>>($"{ServiceName}/{companyId}/accounts", _jsonOptions);
                return response ?? new List<Account>();
            }
            catch (Exception e)
            {
                _logger?.LogError(e, "Error getting accounts for company {CompanyId}", companyId);
                throw;
            }
        }

        public async Task<Account> FindAccountAsync(int accountId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<Account>($"{ServiceName}/FindAccount/{accountId}", _jsonOptions);
                return response ?? throw new Exception("Account not found");
            }
            catch (Exception e)
            {
                _logger?.LogError(e, "Error finding account {AccountId}", accountId);
                throw;
            }
        }
    }
} 