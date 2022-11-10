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
        private readonly ApplicationDbContext _accountContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private PasswordHasher<LoginDTO> _hasher;
        private readonly IConfiguration _config;
        public AuthController(EcommerceDbContext context, IConfiguration config, ApplicationDbContext accountContext,
            RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _accountContext = accountContext;
            _roleManager = roleManager;
            _userManager = userManager;
            _hasher = new();
            _config = config;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate(string? identity)
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
                    IList<string> roles = await _userManager.GetRolesAsync(user);
                    foreach (string role in roles)
                    {
                        tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));
                    }
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var jwtToken = tokenHandler.WriteToken(token);
                    var stringToken = tokenHandler.WriteToken(token);
                    
                    return Ok(stringToken);
                }
                return Unauthorized(identity);
            }
            return Unauthorized(identity);
        }

        [HttpPost]
        public async Task<IActionResult> Register(LoginDTO login)
        {
            IdentityUser? user = await _userManager.FindByEmailAsync(login.username);
            if (user != null) return new BadRequestResult();
            try
            {
                user = new IdentityUser()
                {
                    UserName = login.username,
                    Email = login.username,
                    PasswordHash = _hasher.HashPassword(login, login.password)
                };
                await _userManager.CreateAsync(user);
                await _userManager.AddToRoleAsync(user, "User");
                return Ok();
            }

            catch { return BadRequest(); }
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(LoginDTO login)
        {
            IdentityUser? user = await _userManager.FindByEmailAsync(login.username);
            if(user == null) return Unauthorized(login);
            if (_hasher.VerifyHashedPassword(login, user.PasswordHash, login.password)
                == PasswordVerificationResult.Success)
            {
                return await Authenticate(login.username);
            }

            return Unauthorized(login);
        }

        [HttpPost]
        public async Task<IActionResult> AdminSignIn(LoginDTO login)
        {
            IdentityUser? user = await _userManager.FindByEmailAsync(login.username);
            var roles = await _userManager.GetRolesAsync(user);
            if (user == null || !roles.Contains("SysAdmin")) return Unauthorized(login);
            if (_hasher.VerifyHashedPassword(login, user.PasswordHash, login.password)
                == PasswordVerificationResult.Success)
            {
                return await Authenticate(login.username);
            }

            return Unauthorized(login);
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles(string username)
        {
            //Find user
            IdentityUser? user = await _userManager.FindByEmailAsync(username);
            if(user != null)
            {
                // Get roles of the user
                var roles = await _userManager.GetRolesAsync(user);
                return Ok(roles);
            }
            return NotFound();
        }
    }
}
