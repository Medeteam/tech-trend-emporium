using Data;
using Data.DTOs;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Cors;

namespace App.Controllers
{
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

        // Endpoint to create users with Shopper role (route: api/Auth)
        [HttpPost]
        [Route("api/Auth")]
        [EnableCors("AllowAll")]
        [AllowAnonymous] 
        public async Task<IActionResult> SignupShopper(UserSignupDto userDto)
        {
            return await SignupUser(userDto, "Shopper");
        }

        // Ruta para crear un usuario de tipo Admin (ruta: api/Admin/Auth)
        [HttpPost]
        [Route("api/Admin/Auth")]
        [EnableCors("AllowAll")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> SignupAdmin(UserSignupDto userDto)
        {
            return await SignupUser(userDto, "Admin");
        }

        // Ruta para crear un usuario de tipo Employee (ruta: api/Employee/Auth)
        [HttpPost]
        [Route("api/Employee/Auth")]
        [EnableCors("AllowAll")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> SignupEmployee(UserSignupDto userDto)
        {
            return await SignupUser(userDto, "Employee");
        }

        // Common method to create an user with a specific role
        private async Task<IActionResult> SignupUser(UserSignupDto userDto, string roleName)
        {
            // Verify if username or email alredy exists
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == userDto.Username);
            var existingEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);

            if (existingUser != null || existingEmail != null)
            {
                return Conflict("The username or email is already registered.");
            }

            // Get Role object based on role's name
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
            if (role == null)
            {
                return BadRequest("Invalid role.");
            }

            // Create new user
            var user = new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                Password = userDto.Password,
                SecurityQuestion = userDto.Question,
                SecurityAnswer = userDto.Answer,
                Role = role
            };

            var wishList = new WishList
            {};
            user.WishList = wishList;
            var cart = new Cart { };
            user.Cart = cart;

            // Hash the password
            user.Password = _passwordHasher.HashPassword(user, user.Password);

            // Save user in the database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new {
                message = "User created successfully",
                id = user.User_id,
                email = user.Email,
                username = user.Username
            });
        }
    }
}
