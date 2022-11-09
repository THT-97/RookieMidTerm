using Ecommerce.API.Data;
using Ecommerce.DTO.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly EcommerceDbContext _context;
        private PasswordHasher<LoginDTO> _hasher;
        private readonly IConfiguration _config;
        public AuthController(EcommerceDbContext context, IConfiguration config)
        {
            _context = context;
            _hasher = new();
            _config = config;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody]string? identity)
        {
            if (identity != null)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == identity);
                if (user != null)
                {
                    var issuer = _config["Jwt:Issuer"];
                    var audience = _config["Jwt:Audience"];
                    var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new[]
                        {
                    new Claim("Id", Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, identity),
                    new Claim(JwtRegisteredClaimNames.Email, identity),
                    new Claim(JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString())
                    }),
                        Expires = DateTime.UtcNow.AddMinutes(5),
                        Issuer = issuer,
                        Audience = audience,
                        SigningCredentials = new SigningCredentials
                        (new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha512Signature)
                    };
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var jwtToken = tokenHandler.WriteToken(token);
                    var stringToken = tokenHandler.WriteToken(token);
                    return Ok(stringToken);
                }
                return Unauthorized();
            }
            return Unauthorized();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(LoginDTO login)
        {
            IdentityUser? user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == login.username);
            if (_hasher.VerifyHashedPassword(login, user.PasswordHash, login.password)
                == PasswordVerificationResult.Success)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
