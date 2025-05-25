using AriesContador.Core.Models.Users;
using AriesContador.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Aries.WebAPI.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : AriesBaseController
    {
        private readonly IAdministrationService administrationService;

        public UserController(IAdministrationService administrationService) 
        {
            this.administrationService = administrationService;
        }

        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            try
            {
                var list = administrationService.GetAllUsers();
                return Ok(list); 
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
