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
        public IActionResult GetProducts()
        {
            var products = _context.Products;

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
        [HttpGet("/store/product/{id}")]
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
                return NotFound("Producto no encontrado.");
            }

            return Ok(product);
        }

        [HttpDelete("DeleteProduct/{id}")]
        [Authorize(Policy = "RequireEmployeeOrSuperiorRole")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Product_id == id);

            if (product == null)
            {
                return NotFound("Producto no encontrado.");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok("Producto eliminado con éxito.");
        }
    }
}
