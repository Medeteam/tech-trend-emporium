using Data;
using Data.DTOs;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DBContextTechEmporiumTrend _context;
        private readonly PasswordHasher<User> _passwordHasher;
        public AuthController(DBContextTechEmporiumTrend context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Signup(UserSignupDto userDto)
        {
            {
                // Check if the user already exists
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == userDto.Username);
                var existingEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);

                if (existingUser != null || existingEmail != null)
                {
                    return Conflict("The username or password are alredy registered on the system");
                }


                // Create a new user
                var user = new User
                {
                    Username = userDto.Username,
                    Email = userDto.Email,
                    Password = userDto.Password,
                    Role = new Role { RoleName = "Shopper"}
                };
                user.Password = _passwordHasher.HashPassword(user, user.Password);

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Created();
            }

        }

    }
}
