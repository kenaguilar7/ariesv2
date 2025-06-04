using AriesContador.Core.Models.Accounts;
using AriesContador.Core.Models.Companies;
using AriesContador.Core.Repositories;
using AriesContador.Data.Internal.DataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Linq;
using AriesContador.Core.Models.Utils;
using AriesContador.Core.Models.JournalEntries;
using System.Threading.Tasks;

namespace AriesContador.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IConnectionString _connectionString;
        public AccountRepository(IConnectionString connectionString)
        {
            this._connectionString = connectionString;
        }

        public void Add(Account entity)
        {
            ///if account.AccountType = auxiliar then
            ///Insert new account
            ///Update father account and change AccountType to titulo
            ///Update all journal entries: set new account Id 

            MySqlDataAccess dataAccess = new MySqlDataAccess(_connectionString);
            entity.Id = dataAccess.SaveData<Account, int>("SP_InsertAccount", entity);
        }

        public IEnumerable<Account> FindByCompanyId(string companyId)
        {
            MySqlDataAccess dataAccess = new MySqlDataAccess(_connectionString);
            var output = dataAccess.LoadData<Account, dynamic>("SP_GetAccountsByCompanyId", new { CompanyId = companyId });
            return output;
        }

        public async Task<Account> GetById(int id)
        {
            MySqlDataAccessAsync dataAccess = new MySqlDataAccessAsync(_connectionString);
            var output = await dataAccess.LoadData<Account, dynamic>("SP_GetAccountById", new { accountId = id });
            return output.FirstOrDefault();
        }

        public async Task Remove(Account entity)
        {
            MySqlDataAccessAsync dataAccess = new MySqlDataAccessAsync(_connectionString);
            await dataAccess.SaveData<Account>("SP_DesactivateAccount", entity);
        }

        public void Update(Account entity)
        {
            MySqlDataAccess dataAccess = new MySqlDataAccess(_connectionString);
            dataAccess.SaveData<Account>("SP_UpdateAccount", entity);
        }

        public IEnumerable<Account> GetDefaultAccounts()
        {
            var jsonString = System.IO.File.ReadAllText("defaultaccounts.json");

            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonString)))
            {
                DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(List<Account>));
                List<Account> accounts = (List<Account>)deserializer.ReadObject(ms);
                return accounts;
            }
        }

        public IEnumerable<Account> AccountsWithBalanceByDateRange(BasicReportParam reportParam)
        {
            MySqlDataAccess dataAccess = new MySqlDataAccess(_connectionString);
            var output = dataAccess.LoadData<Account, BasicReportParam>("SP_AuxiliaryAccountsWithBalanceByDateRange", reportParam);
            output.BuildAccountsBalance();
            return output.OrderByTree();
        }

        public Task AddAsync(Account entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Account> GetAccountWithChildBalances(int accountId, string companyId, DateTime startMonth, DateTime endMonth)
        {
            MySqlDataAccessAsync dataAccess = new MySqlDataAccessAsync(_connectionString);
            var parameters = new
            {
                AccountId = accountId,
                CompanyId = companyId,
                StartMonth = $"{startMonth.Year}{startMonth.Month:D2}",
                EndMonth = $"{endMonth.Year}{endMonth.Month:D2}"
            };

            // Get the account and its child accounts with balances
            var accounts = await dataAccess.LoadData<Account, dynamic>("SP_GetAccountHierarchyWithBalances", parameters);
            
            if (!accounts.Any())
                return null;

            // Build the account hierarchy and calculate balances
            accounts.BuildAccountsBalance();

            return accounts.First(a=> a.Id == accountId);
        }
    }
}
