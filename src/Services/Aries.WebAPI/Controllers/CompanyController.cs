using AriesContador.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AriesContador.Core.Models.Companies;

namespace Aries.WebAPI.Controllers
{
    /// <summary>
    /// Controller for managing company-related operations in the Aries system.
    /// </summary>
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CompanyController : AriesBaseController
    {
        private readonly IAdministrationService administrationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyController"/> class.
        /// </summary>
        /// <param name="administrationService">The administration service for company operations.</param>
        public CompanyController(IAdministrationService administrationService)
        {
            this.administrationService = administrationService;
        }

        /// <summary>
        /// Retrieves all companies in the system.
        /// </summary>
        /// <returns>A list of all companies.</returns>
        /// <response code="200">Returns the list of companies successfully.</response>
        /// <response code="500">If there was an internal server error while processing the request.</response>
        [ProducesResponseType(typeof(IEnumerable<Company>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var companies = await administrationService.GetAllCompanies();
                return Ok(companies);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Deletes a company by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the company to delete.</param>
        /// <returns>A response indicating the success of the operation.</returns>
        /// <response code="200">Company was successfully deleted.</response>
        /// <response code="500">If there was an internal server error while processing the request.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var company = new Company { Code = id };
                await administrationService.DeleteCompany(company);
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Generates a new unique company code.
        /// </summary>
        /// <returns>A newly generated company code.</returns>
        /// <response code="200">Returns the newly generated company code.</response>
        /// <response code="500">If there was an internal server error while processing the request.</response>
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("BuildCode")]
        public async Task<IActionResult> BuildNewCode()
        {
            try
            {
                var code = await administrationService.GetCompanyConsecutive();
                return Ok(new { Code = code });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Creates a new company in the system.
        /// </summary>
        /// <param name="company">The company information to create.</param>
        /// <returns>The created company information.</returns>
        /// <response code="200">Returns the newly created company.</response>
        /// <response code="400">If the request data is invalid.</response>
        /// <response code="500">If there was an internal server error while processing the request.</response>
        [ProducesResponseType(typeof(Company), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] Company company)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return HandleValidationError();
                }
                company.UserId = UserId;
                await administrationService.CreateCompany(company);
                return Ok(company);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
