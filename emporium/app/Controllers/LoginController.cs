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
using Microsoft.Identity.Client;

namespace App.Controllers
{
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly DBContextTechEmporiumTrend _context;
        private readonly string _jwtKey;
        private readonly string _jwtIssuer;

        public SessionController(DBContextTechEmporiumTrend context)
        {
            _context = context;
            DotNetEnv.Env.Load();
            _jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
            _jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
        }

        #region Login

        [AllowAnonymous]
        [HttpPost("api/login")]
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
                return Ok(new { token });
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
                new Claim(ClaimTypes.Role, user.RoleName)
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
            var user = _context.Users.FirstOrDefault(u => u.Username == userLogin.Username
                && u.Password == userLogin.Password); // Recuerda que aquí debes almacenar contraseñas hasheadas en lugar de texto plano
            if (user != null)
            {
                var role = GetRoleById(user.Role_id);
                if (role != null)
                {
                    user.Role = role;
                }
            }
            return user;
        }

        private Role? GetRoleById(Guid roleId)
        {
            var role = _context.Roles.FirstOrDefault(r => r.Role_id == roleId);
            return role;
        }

        #endregion

        #region Logout

        [HttpPost("api/logout")]
        [Authorize]
        public IActionResult Logout()
        {
            if (Request.Cookies.ContainsKey("Authorization"))
            {
                Response.Cookies.Delete("Authorization");
                return Ok(new { message = "Logged out successfully" });
            }
            else
            {
                return Ok(new { message = "User is not logged" });
            }
        }

        #endregion
    }
}
