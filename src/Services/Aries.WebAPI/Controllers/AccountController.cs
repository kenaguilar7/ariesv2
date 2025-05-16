using Aries.WebServices.FinancialServices;
using AriesContador.Core.Models.Patterns.Command;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aries.WebAPI.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("{companyId}/accounts")]
        public async Task<IActionResult> FindAccountByCompany(string companyId)
        {
            var accounts = await _accountService.GetAccounts(companyId);
            return Ok(accounts);
        }

        [HttpGet("FindAccount/{accountId}")]
        public async Task<IActionResult> FindAccount(int accountId)
        {
            var account = await _accountService.FindAccount(accountId);
            return Ok(account);
        }
    }
}
