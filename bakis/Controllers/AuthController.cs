using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bakis.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace bakis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private const string SECRET_KEY = "abcdef123564adasdasdasdasdads";
        public static readonly SymmetricSecurityKey SIGNING_KEY = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthController.SECRET_KEY));

        private readonly ProjectContext _context;

        public AuthController(ProjectContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> GetAuth([FromBody] User user)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int user1 = _context.Users.Where(l => l.Login == user.Login).Where(l => l.Password == user.Password).Select(l => l.UserId).FirstOrDefault();
            
            if (user1 == 0)
            {
                return NotFound();
            }
            string id = user1.ToString();
            return Ok(new JwtSecurityTokenHandler().WriteToken(GenerateToken(user.Login, id)));
        }

        private JwtSecurityToken GenerateToken(string username, string id)
        {
            Environment.SetEnvironmentVariable("KEY_FOR_SECRET", "kazkas123213das_ASdasdadasdasdsada");

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("KEY_FOR_SECRET")));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, username));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, id));
            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signingCredentials,
                claims: claims
                );
            return token;
        }
    }
}