using Data.Entities;
using Data.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Data;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;

namespace App.Controllers
{
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly DBContextTechEmporiumTrend _context;
        private readonly string _jwtKey;
        private readonly string _jwtIssuer;
        private readonly PasswordHasher<User> _passwordHasher;

        public SessionController(DBContextTechEmporiumTrend context)
        {
            _context = context;
            DotNetEnv.Env.Load();
            _jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
            _jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
            _passwordHasher = new PasswordHasher<User>();
        }

        #region Login

        // Endpoint to log in all kind of users (route: api/login)
        [AllowAnonymous]
        [EnableCors("AllowAll")]
        [HttpPost("api/login")]
        public IActionResult Login([FromBody] UserLoginDto userLogin)
        {
            if (Request.Cookies.TryGetValue("Authorization", out string cookie))
            {
                var validToken = CheckValidTokenExpiration(cookie);
                if (validToken)
                {
                    return Conflict(new { message = "User alredy logged in" });
                }
            }

            var user = AuthenticateUser(userLogin);

            if (user != null)
            {
                var tokenExpiration = DateTime.Now.AddMinutes(30);
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.None,
                    Expires = tokenExpiration,
                    Secure = true
                };
                var token = GenerateJWTToken(user, tokenExpiration);

                Response.Cookies.Append("Authorization", token, cookieOptions); // Add token to the cookie
                return Ok(new
                {
                    token = token,
                    id = user.User_id,
                    email = user.Email,
                    username = user.Username,
                    role = user.Role.RoleName
                });
            }

            return NotFound("User not found");
        }

        private string GenerateJWTToken(User user, DateTime tokenExpiration)
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
                expires: tokenExpiration,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private User? AuthenticateUser(UserLoginDto userLogin)
        {
            // Search User by the username in the DB
            var user = _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Username == userLogin.Username);

            if (user != null)
            {
                // Use PasswordHasher to compare plain text vs hashed password
                var passwordHasher = new PasswordHasher<User>();
                var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.Password, userLogin.Password);

                // If password is correct, return User
                if (passwordVerificationResult == PasswordVerificationResult.Success)
                {
                    // Load user Role
                    var role = GetRoleById(user.Role_id);
                    if (role != null)
                    {
                        user.Role = role;
                    }
                    return user;
                }
            }

            // If user doesn't exists or password is incorrect, return null
            return null;
        }

        private Role? GetRoleById(Guid roleId)
        {
            var role = _context.Roles.FirstOrDefault(r => r.Role_id == roleId);
            return role;
        }

        private bool CheckValidTokenExpiration(string cookie)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(cookie) as JwtSecurityToken;

            if (jwtToken != null)
            {
                var expClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "exp")?.Value;

                if (expClaim != null && long.TryParse(expClaim, out long exp))
                {
                    var expirationDate = DateTimeOffset.FromUnixTimeSeconds(exp).UtcDateTime;

                    if (expirationDate > DateTime.UtcNow)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion

        #region Logout

        // Endpoint to log out (route: api/logout)
        [HttpPost("api/logout")]
        [EnableCors("AllowAll")]
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

        #region RecoverPassword

        [AllowAnonymous]
        [EnableCors("AllowAll")]
        [HttpPost("api/recover")]
        public async Task<IActionResult> Recover([FromBody] UserRecoverDto userRecover)
        {
            if (Request.Cookies.TryGetValue("Authorization", out string cookie))
            {
                var validToken = CheckValidTokenExpiration(cookie);
                if (validToken)
                {
                    return Conflict(new { message = "User alredy logged in" });
                }
            }
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == userRecover.email);
            if (existingUser == null)
            {
                return Conflict("Not existing user");
            }
            if(existingUser.SecurityAnswer== userRecover.qAnswer)
            {
                existingUser.Password = userRecover.newPassword;
                existingUser.Password = _passwordHasher.HashPassword(existingUser,existingUser.Password);
                _context.Users.Update(existingUser);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Password updated succesfully"});
            }
            return Conflict("Security Answer does not match");

        }

        #endregion
    }
}
