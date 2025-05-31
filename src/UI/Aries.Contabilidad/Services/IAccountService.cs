using Aries.Contabilidad.Models.Accounts;

namespace Aries.Contabilidad.Services
{
    public interface IAccountService
    {
        Task<List<Account>> GetAccountsAsync(string companyId);
        Task<Account> FindAccountAsync(int accountId);
    }
} 