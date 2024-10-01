using App.Services;
using Data.Entities;
using Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("get-categories")]
        public IActionResult GetCategories()
        {
            var categories = _context.Categories
                .Select(c => new
                {
                    c.Category_id,
                    c.Category_name,
                    c.Category_description,
                    c.Created_at,
                })
                .ToList();

            if (!categories.Any())
            {
                return NotFound("No se encontraron categorías en la base de datos.");
            }

            return Ok(categories);
        }

        [HttpGet("GetCategory/{id}")]
        public IActionResult GetCategoryById(Guid id)
        {
            var category = _context.Categories
                .Where(c => c.Category_id == id)
                .Select(c => new
                {
                    c.Category_id,
                    c.Category_name,
                    c.Category_description,
                    c.Created_at,
                })
                .FirstOrDefault();

            if (category == null)
            {
                return NotFound("Categoría no encontrada.");
            }

            return Ok(category);
        }

        [HttpDelete("DeleteCategory/{id}")]
        [Authorize(Policy = "RequireAdminRole")]
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
        [HttpPost("api/category")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto categoryDto)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Category_name == categoryDto.name);
            if (category != null)
            {
                return Conflict("Category already exist");
            }
            var newCategory = new Category
            {
                Category_name = categoryDto.name,
                Category_description = categoryDto.description
            };
            _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync();
            return Ok("Category created Successfully");
        }
    }
}
