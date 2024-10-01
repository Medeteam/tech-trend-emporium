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

        [HttpGet("/category")]
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

            return Ok(categories);
        }

        [HttpGet("/category/{id}")]
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
                return NotFound(new { message = "Category not found" });
            }

            return Ok(category);
        }

        [HttpDelete("/category")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> DeleteCategory([FromBody] Guid id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Category_id == id);

            if (category == null)
            {
                return NotFound(new { message = "Category not found" });
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Category deleted successfully" });
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
