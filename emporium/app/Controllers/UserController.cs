using Data.Entities;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Data.DTOs;

namespace App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly DBContextTechEmporiumTrend _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserController(DBContextTechEmporiumTrend context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        // Método para obtener todos los usuarios (ya implementado)
        [HttpGet("GetUsers")]
        [Authorize(Policy = "RequireAdminRole")]
        public IActionResult GetAllUsers()
        {
            var users = _context.Users
                .Select(u => new
                {
                    u.User_id,
                    u.Username,
                    u.Email,
                    Role = u.Role.RoleName
                })
                .ToList();

            return Ok(users);
        }

        // Obtener un solo usuario por ID
        [HttpGet("GetUser/{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public IActionResult GetUserById(Guid id)
        {
            var user = _context.Users
                .Where(u => u.User_id == id)
                .Select(u => new
                {
                    u.User_id,
                    u.Username,
                    u.Email,
                    Role = u.Role.RoleName
                })
                .FirstOrDefault();

            if (user == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            return Ok(user);
        }

        // Método para filtrar usuarios por su rol
        [HttpGet("GetUsersByRole/{roleName}")]
        [Authorize(Policy = "RequireAdminRole")]
        public IActionResult GetUsersByRole(string roleName)
        {
            var users = _context.Users
                .Where(u => u.Role.RoleName == roleName)  // Filtrar por rol
                .Select(u => new
                {
                    u.User_id,
                    u.Username,
                    u.Email,
                    Role = u.Role.RoleName
                })
                .ToList();

            if (!users.Any())
            {
                return NotFound($"No se encontraron usuarios con el rol '{roleName}'.");
            }

            return Ok(users);
        }



        // Método para eliminar un usuario por ID
        [HttpDelete("DeleteUser/{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = _context.Users.FirstOrDefault(u => u.User_id == id);

            if (user == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok("Usuario eliminado con éxito.");
        }
        [HttpPut("/api/user")]
        public async Task<IActionResult> UpdateUser(UserDto userDto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == userDto.name);

            if (user == null)
            {
                return NotFound("Usuario no encontrado.");
            }
            user.Username = userDto.Username;
            user.Email = userDto.Email;
            user.Password = userDto.Password;
            user.Password = _passwordHasher.HashPassword(user, user.Password);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok("User updated successfully");
        }
    }
}

