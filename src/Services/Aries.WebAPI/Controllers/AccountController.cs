using Aries.WebServices.FinancialServices;
using AriesContador.Core.Models.Patterns.Command;
using AriesContador.Core.Models.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aries.WebAPI.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AccountController : AriesBaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("{companyId}/accounts")]
        public async Task<IActionResult> FindAccountByCompany(string companyId)
        {
            try
            {
                var accounts = await _accountService.GetAccounts(companyId);
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("FindAccount/{accountId}")]
        public async Task<IActionResult> FindAccount(int accountId)
        {
            try
            {
                var account = await _accountService.FindAccount(accountId);
                return Ok(account);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("balance/{accountId}")]
        public async Task<IActionResult> GetAccountBalances(
            int accountId,
            [FromQuery] string companyId,
            [FromQuery] DateTime startMonth,
            [FromQuery] DateTime endMonth)
        {
            try
            {
                var account = await _accountService.GetAccountBalances(
                    accountId,
                    companyId,
                    startMonth,
                    endMonth);
                    
                return Ok(account);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
