using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BE.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace BE.Controllers
{
    [ExcludeFromCodeCoverage]
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

            User userDB = _context.Users.Where(l => l.Login == user.Login).FirstOrDefault();
            if (userDB == null || !verifyPass(userDB, user.Password))
            {
                return NotFound();
            }
            string id = userDB.UserId.ToString();
            string rights = generateRightsString(userDB);
            return Ok(new JwtSecurityTokenHandler().WriteToken(GenerateToken(user.Login, id, rights)));
        }

        private bool verifyPass(User user, string pass)
        {
            bool verified = false;
            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.Password, pass);
            if (result == PasswordVerificationResult.Success) verified = true;
            else if (result == PasswordVerificationResult.SuccessRehashNeeded) verified = true;
            else if (result == PasswordVerificationResult.Failed) verified = false;
            return verified;
        }

        private string generateRightsString(User user)
        {
            user.GroupRight = _context.GroupRights.Where(l => l.GroupRightId == user.GroupRightId).FirstOrDefault();
            string rights = "0000000";
            var temp = rights.ToCharArray();
            if (user.GroupRight.manageClassifiers)
                temp[0] = '1';
            if (user.GroupRight.manageContests)
                temp[1] = '1';
            if (user.GroupRight.manageCustomers)
                temp[2] = '1';
            if (user.GroupRight.manageEmployees)
                temp[3] = '1';
            if (user.GroupRight.manageProjects)
                temp[4] = '1';
            if (user.GroupRight.manageTenders)
                temp[5] = '1';
            if (user.GroupRight.manageUsers)
                temp[6] = '1';
            string s = new string(temp);
            return s;
        }

        private JwtSecurityToken GenerateToken(string username, string id, string rights)
        {
            Environment.SetEnvironmentVariable("KEY_FOR_SECRET", "kazkas123213das_ASdasdadasdasdsada");

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("KEY_FOR_SECRET")));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, username));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, id));
            claims.Add(new Claim("Rights", rights));
            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signingCredentials,
                claims: claims
                );
            return token;
        }
    }
}