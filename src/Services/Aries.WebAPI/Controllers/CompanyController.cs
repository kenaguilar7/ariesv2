﻿using AriesContador.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AriesContador.Core.Models.Companies;

namespace Aries.WebAPI.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly IAdministrationService administrationService;

        public CompanyController(IAdministrationService administrationService)
        {
            this.administrationService = administrationService;
        }

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
                Console.WriteLine($"Error in GetAll: {ex}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

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
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

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
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] Company company)
        {
            try
            {
                await administrationService.CreateCompany(company);
                return Ok(company);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
