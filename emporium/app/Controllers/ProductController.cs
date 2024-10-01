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
    public class ProductController : ControllerBase
    {
        private readonly DBContextTechEmporiumTrend _context;

        public ProductController(DBContextTechEmporiumTrend context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet("/store/products")]
        public IActionResult GetProducts([FromQuery] string category = "")
        {
            var products = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                if (_context.Categories.Where(c => c.Category_name == category).Any())
                {
                    products = products.Where(p => p.Category.Category_name == category);
                }
                else
                {
                    return NotFound(new { message = "The category doesn't exists" });
                }
            }

            // TODO actualizar para agregar el rating
            // Retornar la lista de productos con los detalles de usuario y estado de trabajo
            var productDtos = products.Select(p => new ProductDto
            {
                id = p.Product_id,
                title = p.Name,
                description = p.Description,
                category = p.Category.Category_name,
                image = p.Image,
                price = p.Price,
                stock = p.Stock,
            }).ToList();

            return Ok(productDtos);
        }

        [AllowAnonymous]
        [HttpGet("/store/products/{id}")]
        public IActionResult GetProductById(Guid id)
        {
            // TODO actualizar para agregar el rating
            var product = _context.Products
                .Where(p => p.Product_id == id)
                .Select(p => new ProductDto
                {
                    id = p.Product_id,
                    title = p.Name,
                    description = p.Description,
                    category = p.Category.Category_name,
                    image = p.Image,
                    price = p.Price,
                    stock = p.Stock,
                })
                .FirstOrDefault();

            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }

            return Ok(product);
        }        

        [HttpDelete("/products")]
        [Authorize(Policy = "RequireEmployeeOrSuperiorRole")]
        public async Task<IActionResult> DeleteProduct([FromBody]Guid id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Product_id == id);

            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Product deleted successfuly" });
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
        [HttpPut("api/Product")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductDto productDto)
        {
            var existingProduct = _context.Products.FirstOrDefault(p => p.Product_id == productDto.id);
            if (existingProduct == null)
            {
                return Conflict("No existing product");
            }
            var existingCategory = _context.Categories.FirstOrDefault(c => c.Category_name == productDto.category);
            if(existingCategory== null)
            {
                return Conflict("No existing category");
            }
            existingProduct.Name = productDto.title;
            existingProduct.Description = productDto.description;
            existingProduct.Image = productDto.image;
            existingProduct.Stock = productDto.stock;
            existingProduct.Price = productDto.price;
            existingProduct.CategoryName = productDto.category;
            _context.Products.Update(existingProduct);
            await _context.SaveChangesAsync();
            return Ok("Product Updated Succesfully");
        }

    }
}
