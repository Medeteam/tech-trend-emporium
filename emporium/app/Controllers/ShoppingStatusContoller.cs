using Data.Entities;
using Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Win32.SafeHandles;
using Microsoft.AspNetCore.Authorization;

namespace App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShoppingStatusController : ControllerBase
    {
        private readonly DBContextTechEmporiumTrend _context;

        public ShoppingStatusController(DBContextTechEmporiumTrend context)
        {
            _context = context;
        }

        [HttpPost("create-default-shopping-status")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> CreateDefaultShoppingStatus()
        {
            var defaultShoppingStatuses = new List<string> { "Created", "Accepted", "Declined" };

            var existingShoppingStatuses = _context.ShoppingStatus
                                        .Where(Ss => defaultShoppingStatuses.Contains(Ss.Shopping_status))
                                        .Select(Ss => Ss.Shopping_status)
                                        .ToList();

            // Filtrar los estados que aún no existen
            var shoppingStatusesToCreate = defaultShoppingStatuses.Except(existingShoppingStatuses).ToList();

            // Crear y agregar los estados que faltan
            foreach (var ShoppingStatus in shoppingStatusesToCreate)
            {
                var newShoppingStatus = new ShoppingStatus { Shopping_status = ShoppingStatus };
                _context.ShoppingStatus.Add(newShoppingStatus);
            }

            // Guardar los cambios en la base de datos
            if (shoppingStatusesToCreate.Count > 0)
            {
                await _context.SaveChangesAsync();
                return Ok($"Shopping Status created: {string.Join(", ", shoppingStatusesToCreate)}");
            }
            else
            {
                return Ok("All Shopping statuses created");
            }
        }

        [HttpGet(Name = "GetShoppingStatuses")]
        [Authorize(Policy = "RequireAdminRole")]
        public IActionResult GetShoppingStatuses()
        {
            // Obtener todos los estados desde la base de datos usando LINQ
            var shoppingStatuses = _context.ShoppingStatus.ToList();

            if (shoppingStatuses == null || shoppingStatuses.Count == 0)
            {
                return NotFound("Not status found in db.");
            }

            return Ok(shoppingStatuses);
        }

        [HttpGet("GetShoppingStatus/{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public IActionResult GetShoppingStatusById(Guid id)
        {
            var shoppingStatus = _context.ShoppingStatus.FirstOrDefault(ss => ss.Shopping_status_id == id);

            if (shoppingStatus == null)
            {
                return NotFound("Shopping status not found.");
            }

            return Ok(shoppingStatus);
        }

        [HttpDelete("DeleteShoppingStatus/{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> DeleteShoppingStatus(Guid id)
        {
            var shoppingStatus = _context.ShoppingStatus.FirstOrDefault(ss => ss.Shopping_status_id == id);

            if (shoppingStatus == null)
            {
                return NotFound("Shopping status not found.");
            }

            _context.ShoppingStatus.Remove(shoppingStatus);
            await _context.SaveChangesAsync();

            return Ok("Shopping status deleted successfully.");
        }
    }
}
