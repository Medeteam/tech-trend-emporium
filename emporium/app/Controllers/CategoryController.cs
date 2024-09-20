using App.Services;
using Data.Entities;
using Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly FakeStoreService _fakeStoreService;
        private readonly DBContextTechEmporiumTrend _context;
        private readonly Random _random;

        public CategoryController(FakeStoreService fakeStoreService, DBContextTechEmporiumTrend context)
        {
            _fakeStoreService = fakeStoreService;
            _context = context;
            _random = new Random();
        }

        [HttpPost("sync-categories")]
        public async Task<IActionResult> SyncCategories()
        {
            // Obtener las categorías desde la API de FakeStore
            var categoriesFromApi = await _fakeStoreService.GetCategoriesAsync();

            // Obtener todos los usuarios disponibles
            var users = _context.Users.ToList();

            // Obtener el estado de trabajo 'Accepted'
            var acceptedStatus = _context.Jobs.FirstOrDefault(js => js.Job_status == "Accepted");

            if (!users.Any() || acceptedStatus == null)
            {
                return BadRequest("Es necesario tener usuarios y un estado de trabajo 'Accepted' antes de sincronizar categorías.");
            }

            // Obtener las categorías ya existentes en la base de datos
            var existingCategories = _context.Categories.Select(c => c.Category_name).ToList();

            // Filtrar las categorías que aún no existen en la base de datos
            var newCategories = categoriesFromApi
                .Where(apiCategory => !existingCategories.Contains(apiCategory))
                .Select(apiCategory => new Category
                {
                    Category_name = apiCategory,
                    Category_description = $"Categoría agregada desde API, sin descripción adicional.",
                    User_id = users[_random.Next(users.Count)].User_id, // Seleccionar un usuario aleatorio
                    Job_status_id = acceptedStatus.Job_status_id // Asignar el estado 'Accepted'
                })
                .ToList();

            // Si hay categorías nuevas, agregarlas a la base de datos
            if (newCategories.Any())
            {
                _context.Categories.AddRange(newCategories);
                await _context.SaveChangesAsync();

                return Ok($"{newCategories.Count} categorías sincronizadas con éxito.");
            }
            else
            {
                return Ok("No hay categorías nuevas para sincronizar.");
            }
        }

        [HttpGet("get-categories")]
        public IActionResult GetCategories()
        {
            var categories = _context.Categories
                .Include(c => c.User) // Incluir el usuario relacionado
                .Include(c => c.Job_status) // Incluir el estado de trabajo relacionado
                .Select(c => new
                {
                    c.Category_id,
                    c.Category_name,
                    c.Category_description,
                    c.Created_at,
                    UserName = c.User.Username, // Mostrar el nombre del usuario
                    JobStatus = c.Job_status.Job_status // Mostrar el estado de trabajo
                })
                .ToList();

            if (!categories.Any())
            {
                return NotFound("No se encontraron categorías en la base de datos.");
            }

            return Ok(categories);
        }
    }
}
