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
    public class ProductController : ControllerBase
    {
        private readonly FakeStoreService _fakeStoreService;
        private readonly DBContextTechEmporiumTrend _context;
        private readonly Random _random;

        public ProductController(FakeStoreService fakeStoreService, DBContextTechEmporiumTrend context)
        {
            _fakeStoreService = fakeStoreService;
            _context = context;
            _random = new Random();
        }

        [HttpPost("sync-products")]
        public async Task<IActionResult> SyncProducts()
        {
            // Obtener los productos desde la Fake Store API
            var productsFromApi = await _fakeStoreService.GetProductsAsync();

            // Obtener todos los usuarios disponibles
            var users = _context.Users.ToList();

            // Obtener el shopping status 'Accepted'
            var acceptedStatus = _context.Jobs.FirstOrDefault(js => js.Job_status == "Accepted");

            if (!users.Any() || acceptedStatus == null)
            {
                return BadRequest("Es necesario tener usuarios y un estatus 'Accepted' antes de sincronizar productos.");
            }

            // Filtrar los productos que aún no existen en la base de datos
            var existingProductNames = _context.Products.Select(p => p.Name).ToList();

            var newProducts = productsFromApi
                .Where(apiProduct => !existingProductNames.Contains(apiProduct.Name))
                .Select(apiProduct => new Product
                {
                    Name = apiProduct.Name, // Mapeo directo desde la API
                    Description = apiProduct.Description, // Mapeo directo desde la API
                    Image = apiProduct.Image, // Mapeo directo desde la API
                    Price = apiProduct.Price, // Mapeo directo desde la API
                    Stock = 5, // Asignar un stock predeterminado
                    User_id = users[_random.Next(users.Count)].User_id, // Seleccionar aleatoriamente un usuario de la lista
                    Job_status_id = acceptedStatus.Job_status_id, // Siempre asignar el estatus 'Accepted'
                    Wishlist_id = null // Mantener la wishlist vacía
                })
                .ToList();

            // Agregar todos los productos nuevos a la base de datos
            if (newProducts.Any())
            {
                _context.Products.AddRange(newProducts);
                await _context.SaveChangesAsync();

                return Ok($"{newProducts.Count} productos sincronizados con éxito.");
            }
            else
            {
                return Ok("No hay productos nuevos para sincronizar.");
            }
        }

        [HttpGet("Get-Products")]
        public IActionResult GetProducts()
        {
            var products = _context.Products;

            // Retornar la lista de productos con los detalles de usuario y estado de trabajo
            var productDtos = products.Select(p => new
            {
                product_id = p.Product_id,
                title = p.Name,
                description = p.Description,
                image = p.Image,
                price = p.Price,
                stock = p.Stock,
                created_at = p.Created_at,
                username =  p.User.Username , 
                job_status = p.Job_status.Job_status,
            }).ToList();

            return Ok(productDtos);
        }

        [HttpGet("GetProduct/{id}")]
        public IActionResult GetProductById(Guid id)
        {
            var product = _context.Products
                .Include(p => p.User)
                .Include(p => p.Job_status)
                .Where(p => p.Product_id == id)
                .Select(p => new
                {
                    product_id = p.Product_id,
                    title = p.Name,
                    description = p.Description,
                    image = p.Image,
                    price = p.Price,
                    stock = p.Stock,
                    created_at = p.Created_at,
                    username = p.User.Username,
                    job_status = p.Job_status.Job_status
                })
                .FirstOrDefault();

            if (product == null)
            {
                return NotFound("Producto no encontrado.");
            }

            return Ok(product);
        }

        [HttpDelete("DeleteProduct/{id}")]
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
