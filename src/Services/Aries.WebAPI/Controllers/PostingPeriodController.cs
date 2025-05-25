using Aries.WebServices.FinancialServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aries.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PostingPeriodController : AriesBaseController
    {
        private readonly IPostingPeriodService _postingPeriodService;

        public PostingPeriodController(IPostingPeriodService postingPeriodService)
        {
            _postingPeriodService = postingPeriodService;
        }

        [HttpGet("GetPostingPeriods/{companyId}")]
        public async Task<IActionResult> GetPostingPeriods(string companyId)
        {
            try
            {
                var result = await _postingPeriodService.GetPostingPeriods(companyId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
