using Data.Entities;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;

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

        [HttpPost("Create-Admin-Users")]
        public async Task<IActionResult> CreateAdminUsers()
        {
            // Verificar si existe el rol de Admin
            var adminRole = _context.Roles.FirstOrDefault(r => r.RoleName == "Admin");
            if (adminRole == null)
            {
                return BadRequest("Admin role not found");
            }

            var usersToCreate = new List<User>
            {
            new User
                {
                    User_id = Guid.NewGuid(),
                    Username = "CamiloVelezP",
                    Email = "CamiVelezP@gmail.com",
                    Password = "EndInterns20242",
                    Role_id = adminRole.Role_id
                },
                new User
                {
                    User_id = Guid.NewGuid(),
                    Username = "JhonatanT",
                    Email = "JhonatanT@gmail.com",
                    Password = "EndInterns20242",
                    Role_id = adminRole.Role_id
                },
                new User
                {
                    User_id = Guid.NewGuid(),
                    Username = "Sariasu",
                    Email = "SebasAriasU@Gmail.com",
                    Password = "EndInterns20242",
                    Role_id = adminRole.Role_id
                }
            };
            

            foreach (var user in usersToCreate)
            {
                // Verificar si ya existe un usuario con el mismo Email o Username
                var existingUser = _context.Users
                    .FirstOrDefault(u => u.Email == user.Email || u.Username == user.Username);

                if (existingUser != null)
                {
                    return Ok("Los usuarios ya existen");
                }

                // Hashear la contraseña antes de guardar
                user.Password = _passwordHasher.HashPassword(user, user.Password);

                // Añadir el usuario a la base de datos
                _context.Users.Add(user);
            }

            // Guardar los cambios
            await _context.SaveChangesAsync();

            return Ok("Usuarios administradores creados con éxito.");
        }

        [HttpGet("GetUsers")]
        public IActionResult GetAllUsers()
        {
            var users = _context.Users
                .Select(u => new
                {
                    u.User_id,
                    u.Username,
                    u.Email,
                    Role = u.Role.RoleName, // Mostrar el nombre del rol
                })
                .ToList();

            return Ok(users);
        }
    }
}
