using Data;
using Data.DTOs;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    public class JobProductController : ControllerBase
    {
        private readonly DBContextTechEmporiumTrend _context;

        public JobProductController(DBContextTechEmporiumTrend context)
        {
            _context = context;
        }
        [HttpPost("api/Product")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.title,
                Description = productDto.description,
                Image = productDto.image,
                Price = productDto.price,
                Stock = productDto.stock,
                CategoryName = productDto.category
            };
            var category = _context.Categories.FirstOrDefault(c => c.Category_name == product.CategoryName);
            if (category == null)
            {
                return Conflict("No category found for this product");
            }
            product.Category_id = category.Category_id;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return Ok("Product Created Successfully");
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
