using Data.Entities;
using Data.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DotNetEnv;
using System.Security.Claims;
using Data;
using System.IdentityModel.Tokens.Jwt;
using Data.DataForTest;
using Microsoft.Identity.Client;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly DBContextTechEmporiumTrend _context;
        private readonly string _jwtKey;
        private readonly string _jwtIssuer;

        public LoginController(DBContextTechEmporiumTrend context)
        {
            _context = context;
            DotNetEnv.Env.Load();
            _jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
            _jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] UserLoginDto userLogin)
        {
            var user = AuthenticateUser(userLogin);

            if (user != null)
            {
                var token = GenerateJWTToken(user);
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict, 
                    Expires = DateTime.Now.AddMinutes(120) 
                };

                Response.Cookies.Append("Authorization", token, cookieOptions); // Añadir el token a la cookie
                return Ok(token);
            }
            return NotFound("User not found");
        }

        private string GenerateJWTToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(ClaimTypes.Sid , user.User_id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.RoleName)
            };

            var token = new JwtSecurityToken(
                issuer: _jwtIssuer,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private User? AuthenticateUser(UserLoginDto userLogin)
        {
            // Real search
            //var user = _context.Users.FirstOrDefault(u => u.Username == userLogin.Username 
            //    && u.Password == userLogin.Password);
            //return user is not null ? user : null;

            // Test search
            var currentUser = UserConstants.Users.FirstOrDefault(o => o.Username.ToLower() == userLogin.Username.ToLower() && o.Password == userLogin.Password);

            if (currentUser != null)
            {
                return currentUser;
            }
            return null;
        }
    }
}
