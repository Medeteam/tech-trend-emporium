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

        // Ruta para crear un usuario de tipo Shopper (ruta: api/Auth)
        [HttpPost]
        [Route("api/Auth")]
        [AllowAnonymous] 
        public async Task<IActionResult> SignupShopper(UserSignupDto userDto)
        {
            return await SignupUser(userDto, "Shopper");
        }

        // Ruta para crear un usuario de tipo Admin (ruta: api/Admin/Auth)
        [HttpPost]
        [Route("api/Admin/Auth")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> SignupAdmin(UserSignupDto userDto)
        {
            return await SignupUser(userDto, "Admin");
        }

        // Ruta para crear un usuario de tipo Employee (ruta: api/Employee/Auth)
        [HttpPost]
        [Route("api/Employee/Auth")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> SignupEmployee(UserSignupDto userDto)
        {
            return await SignupUser(userDto, "Employee");
        }

        // Método común para crear un usuario con el rol proporcionado
        private async Task<IActionResult> SignupUser(UserSignupDto userDto, string roleName)
        {
            // Verificar si el usuario o el correo ya existen
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == userDto.Username);
            var existingEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);

            if (existingUser != null || existingEmail != null)
            {
                return Conflict("The username or email is already registered.");
            }

            // Obtener el rol basado en el nombre del rol
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
            if (role == null)
            {
                return BadRequest("Invalid role.");
            }

            // Crear un nuevo usuario
            var user = new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                Password = userDto.Password,
                Role_id = role.Role_id
            };

            var wishList = new WishList
            {};
            user.WishList = wishList;

            // Hashear la contraseña
            user.Password = _passwordHasher.HashPassword(user, user.Password);

            // Guardar el usuario en la base de datos
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "User created successfully", UserId = user.User_id });
        }
    }
}
