using Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebScraping.Core.Domain.Entities;
using WebScraping.Intrastructure.Persistence.DbContexts;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public LoginController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserLogin data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            User user = await _context.Users.Where(i => i.UserName == data.UserName).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound("User doesn't exist");
            }

            if (data.Password != user.Password)
            {
                return new ObjectResult("Invalid Password") { StatusCode = 403 };
            }
            else
            {
                var secretKey = _configuration.GetValue<string>("SecretKey");
                var key = Encoding.ASCII.GetBytes(secretKey);

                var claims = new ClaimsIdentity();
                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Name));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

                };

                var tokenHadler = new JwtSecurityTokenHandler();
                var createdToken = tokenHadler.CreateToken(tokenDescriptor);
                string bearerToken = tokenHadler.WriteToken(createdToken);

                return Ok(bearerToken);

            }
        }
    }
}
