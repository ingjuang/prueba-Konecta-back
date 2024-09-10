using Business.Interfaces;
using Core.DTOs;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private IConfiguration _configuration;
        private string jwtToken = null;
        private User? user = null;

        public LoginController(ILoginService loginService, IConfiguration configuration)
        {
            _loginService = loginService;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            user = await _loginService.GetUser(dto);
            if (user is null)
                return Unauthorized(new { message = "Credenciales inválidas." });

            jwtToken = GenerateToken(user);
            return Ok(new { token = jwtToken, user = new { user.Name, user.UserName,  user.Email } });
        }

        [Authorize]
        [HttpGet, Route("CheckToken")]
        public async Task<IActionResult> Login()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var name = identity.FindFirst(ClaimTypes.Name)?.Value;
                var userName = identity.FindFirst("UserName")?.Value;
                var userEmail = identity.FindFirst(ClaimTypes.Email)?.Value;
                var newToken = GenerateToken(new User
                {
                    Name = name,
                    UserName = userName,
                    Email = userEmail
                });

                return Ok(new
                {
                    token = newToken,
                    user = new { Name = name, UserName = userName, Email = userEmail }
                });
            }

            return Unauthorized(new { message = "Token no válido." });
        }

        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("UserName", user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Key").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var securityToken = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(60),
                    signingCredentials: creds);

            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }
    }
}
