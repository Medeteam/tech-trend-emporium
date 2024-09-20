using Data.Entities;
using Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly DBContextTechEmporiumTrend _context;

        public RoleController(DBContextTechEmporiumTrend context)
        {
            _context = context;
        }

        [HttpPost("create-default-roles")]
        public async Task<IActionResult> CreateDefaultRoles()
        {
            var defaultRoles = new List<string> { "Admin", "Employee", "Shopper" };

            var existingRoles = _context.Roles
                                        .Where(r => defaultRoles.Contains(r.RoleName))
                                        .Select(r => r.RoleName)
                                        .ToList();

            // Filtrar los roles que aún no existen
            var rolesToCreate = defaultRoles.Except(existingRoles).ToList();

            // Crear y agregar los roles que faltan
            foreach (var roleName in rolesToCreate)
            {
                var newRole = new Role { RoleName = roleName };
                _context.Roles.Add(newRole);
            }

            // Guardar los cambios en la base de datos
            if (rolesToCreate.Count > 0)
            {
                await _context.SaveChangesAsync();
                return Ok($"Roles creados: {string.Join(", ", rolesToCreate)}");
            }
            else
            {
                return Ok("Todos los roles ya existen en la base de datos.");
            }
        }

        [HttpGet("GetRole")]
        public IActionResult GetRoles()
        {
            // Obtener todos los roles desde la base de datos usando LINQ
            var roles = _context.Roles.ToList();

            if (roles == null || roles.Count == 0)
            {
                return NotFound("No se encontraron roles en la base de datos.");
            }

            return Ok(roles);
        }

        [HttpGet("GetRole/{id}")]
        public IActionResult GetRoleById(Guid id)
        {
            var role = _context.Roles.FirstOrDefault(r => r.Role_id == id);

            if (role == null)
            {
                return NotFound("Rol no encontrado.");
            }

            return Ok(role);
        }

        [HttpDelete("DeleteRole/{id}")]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            var role = _context.Roles.FirstOrDefault(r => r.Role_id == id);

            if (role == null)
            {
                return NotFound("Rol no encontrado.");
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            return Ok("Rol eliminado con éxito.");
        }
    }
}
