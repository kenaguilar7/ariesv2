using Aries.Contabilidad.Models.Accounts;

namespace Aries.Contabilidad.Services
{
    public interface IAccountService
    {
        Task<List<Account>> GetAccountsAsync();
    }
} 