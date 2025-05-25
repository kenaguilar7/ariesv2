using AriesContador.Core.Models.Users;
using AriesContador.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Aries.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : AriesBaseController
    {
        private readonly IConfiguration _config;
        private readonly IAdministrationService _administrationService;

        public AuthController(IConfiguration config, IAdministrationService administrationService)
        {
            _config = config;
            _administrationService = administrationService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody]Login userLogin)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return HandleValidationError();
                }

                var users = _administrationService.GetAllUsers();
                var user = users.FirstOrDefault(u => u.UserName.ToLower() == userLogin.UserId.ToLower() && u.Password == userLogin.Password);
                
                if (user != null)
                {
                    var tokenString = GenerateToken(user);
                    var webToken = new WebToken()
                    {
                        Token = tokenString,
                        User = user
                    }; 

                    return Ok(webToken);
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        private string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim("UserId", user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name)
                }),
                Expires = DateTime.UtcNow.AddDays(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
