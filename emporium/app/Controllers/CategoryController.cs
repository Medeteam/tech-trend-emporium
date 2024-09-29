using App.Services;
using Data.Entities;
using Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Data.DTOs;

namespace App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly DBContextTechEmporiumTrend _context;

        public CategoryController(DBContextTechEmporiumTrend context)
        {
            _context = context;
        }

        [HttpGet("category")]
        public IActionResult GetCategories()
        {
            var categories = _context.Categories
                .Select(c => new CategoryDto
                {
                    id = c.Category_id,
                    name = c.Category_name,
                    description = c.Category_description
                })
                .ToList();

            if (!categories.Any())
            {
                return NotFound("No se encontraron categorías en la base de datos.");
            }

            return Ok(categories);
        }

        [HttpGet("category/{id}")]
        public IActionResult GetCategoryById(Guid id)
        {
            var category = _context.Categories
                .Where(c => c.Category_id == id)
                .Select(c => new CategoryDto
                {
                    id = c.Category_id,
                    name = c.Category_name,
                    description = c.Category_description
                })
                .FirstOrDefault();

            if (category == null)
            {
                return NotFound("Categoría no encontrada.");
            }

            return Ok(category);
        }

        [HttpDelete("category/{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Category_id == id);

            if (category == null)
            {
                return NotFound("Categoría no encontrada.");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return Ok("Categoría eliminada con éxito.");
        }

        [HttpGet("GetPeoductsByCategory/{category}")]
        public async Task<IActionResult> GetProductByCategory(string category)
        {
            var products = await _context.Products.Where(
                p => p.Category.Category_name == category).ToListAsync();
            if (!products.Any())
            {
                return NotFound(new { message = "No products found in this category." });
            }

            return Ok(products);
        }
    }
}
