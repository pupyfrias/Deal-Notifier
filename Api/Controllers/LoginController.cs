using Api.Models;
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


namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly contextItem _context;
        private readonly IConfiguration _configuration;
        public LoginController(contextItem context, IConfiguration configuration)
        {
            this._context = context;
            this._configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserLogin data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            User user = await _context._User.Where(i => i.User_Name == data.User_name).FirstOrDefaultAsync();

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
