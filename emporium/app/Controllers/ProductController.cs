using Data.Entities;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Data.DTOs;

namespace App.Controllers
{
    [ApiController]
    [Route("/api")]
    public class ProductController : ControllerBase
    {
        private readonly DBContextTechEmporiumTrend _context;

        public ProductController(DBContextTechEmporiumTrend context)
        {
            _context = context;
        }

        // Endpoint to get all products. Can be filtered by category (route: api/store/products)
        [AllowAnonymous]
        [HttpGet("store/products")]
        public IActionResult GetProducts([FromQuery] string category = "")
        {
            var products = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                if (_context.Categories.Any(c => c.Category_name == category))
                {
                    products = products.Where(p => p.Category != null && p.Category.Category_name == category);
                }
                else
                {
                    return NotFound(new { message = "The category doesn't exists" });
                }
            }

            // Return list of products with its details
            var productDtos = products.Select(p => new ProductDto
            {
                id = p.Product_id,
                title = p.Name,
                description = p.Description,
                category = p.Category.Category_name,
                image = p.Image,
                price = p.Price,
                stock = p.Stock
            }).ToList();

            var reviewsList = _context.Reviews.ToList();
            foreach (var product in productDtos) {
                var reviews = reviewsList
                    .Where(r => r.Product_id == product.id)
                    .ToList();
                var rate = reviews.Any() ? reviews.Average(r => r.Review_rate) : 0;
                var count = reviews.Count();
                product.rating = new RatingDto
                {
                    Rate = (decimal)rate,
                    Count = count
                };
            }

            return Ok(productDtos);
        }

        // Endpoint to get a specific product (route: api/store/products/{id})
        [AllowAnonymous]
        [HttpGet("store/products/{id}")]
        public IActionResult GetProductById(Guid id)
        {
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

            var reviews = _context.Reviews
                .Where(r => r.Product_id == product.id)
                .ToList();
            var rate = reviews.Any() ? reviews.Average(r => r.Review_rate) : 0;
            var count = reviews.Count();
            product.rating = new RatingDto
            {
                Rate = (decimal)rate,
                Count = count
            };

            return Ok(product);
        }

        // Endpoint to delete a product (route: api/product)
        [HttpDelete("product")]
        [Authorize(Policy = "RequireEmployeeOrSuperiorRole")]
        public async Task<IActionResult> DeleteProduct([FromBody] DeleteProductRequestDto requestDto)
        {
            var product = _context.Products.FirstOrDefault(p => p.Product_id == requestDto.id);

            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Product deleted successfuly" });
        }

        // Endpoint to create a product (route: api/product)
        [HttpPost("product")]
        [Authorize(Policy = "RequireEmployeeOrSuperiorRole")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDto productDto)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Category_name == productDto.category);
            if (category == null)
            {
                return Conflict(new { message = "No category found for this product" });
            }
            if (_context.Products.Any(p => p.Name.ToLower() == productDto.title.ToLower()))
            {
                return Conflict(new { message = "There is a product with the same name" });
            }

            var product = new Product
            {
                Name = productDto.title,
                Description = productDto.description,
                Image = productDto.image,
                Price = productDto.price,
                Stock = productDto.stock,
                Category = category
            };
            _context.Add(product);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                productId = product.Product_id,
                message = "Product Created Successfully"
            });
        }

        // Endpoint to update a product (route: api/product)
        [HttpPut("product")]
        [Authorize(Policy = "RequireEmployeeOrSuperiorRole")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductDto productDto)
        {
            var existingProduct = _context.Products.FirstOrDefault(p => p.Product_id == productDto.id);
            if (existingProduct == null)
            {
                return Conflict(new { message = "No existing product" });
            }
            var existingCategory = _context.Categories.FirstOrDefault(c => c.Category_name == productDto.category);
            if(existingCategory == null)
            {
                return Conflict(new { message = "No existing category" });
            }
            existingProduct.Name = productDto.title;
            existingProduct.Description = productDto.description;
            existingProduct.Image = productDto.image;
            existingProduct.Stock = productDto.stock;
            existingProduct.Price = productDto.price;
            existingProduct.Category = existingCategory;
            _context.Update(existingProduct);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Product Updated Succesfully" });
        }

    }
}
