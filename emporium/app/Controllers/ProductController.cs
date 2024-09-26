using App.Services;
using Data.Entities;
using Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly FakeStoreService _fakeStoreService;
        private readonly DBContextTechEmporiumTrend _context;

        public ProductController(FakeStoreService fakeStoreService, DBContextTechEmporiumTrend context)
        {
            _fakeStoreService = fakeStoreService;
            _context = context;
        }

        [HttpPost("sync-products")]
        [Authorize(Policy = "RequireEmployeeOrSuperiorRole")]
        public async Task<IActionResult> SyncProducts()
        {
            // Obtener los productos desde la Fake Store API
            var productsFromApi = await _fakeStoreService.GetProductsAsync();

            // Buscar al usuario específico "CamiloVelezP"
            var user = _context.Users.FirstOrDefault(u => u.Username == "CamiloVelezP");

            // Obtener el estado de trabajo 'Accepted'
            var acceptedStatus = _context.Jobs.FirstOrDefault(js => js.Job_status == "Accepted");

            // Validar si se encontró el usuario y el estado
            if (user == null || acceptedStatus == null)
            {
                return BadRequest("Es necesario tener el usuario 'CamiloVelezP' y un estado de trabajo 'Accepted' antes de sincronizar productos.");
            }

            //Obtener las categorias de la base de datos
            var categories = GetCategories();

            // Filtrar los productos que aún no existen en la base de datos
            var existingProductNames = _context.Products.Select(p => p.Name).ToList();

            var newProducts = new List<Product>();

            foreach (var apiProduct in productsFromApi.Where(p => !existingProductNames.Contains(p.Name)))
            {
                
                var category = categories.FirstOrDefault(c => c.Category_name == apiProduct.CategoryName);
                // Crear el nuevo producto
                var newProduct = new Product
                {
                    Name = apiProduct.Name,
                    Description = apiProduct.Description,
                    Category = category,
                    //Category_id = category.Category_id, // Usar la Category_id existente o recién creada
                    Image = apiProduct.Image,
                    Price = apiProduct.Price,
                    Stock = 5, // Asignar un stock predeterminado
                    User_id = user.User_id, // Asignar siempre el User_id de "CamiloVelezP"
                    Job_status_id = acceptedStatus.Job_status_id, // Siempre asignar el estatus 'Accepted'
                    Wishlist_id = null // Mantener la wishlist vacía
                };

                newProducts.Add(newProduct);
            }

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

        private List<Category> GetCategories()
        {
            var categories = _context.Categories.ToList<Category>();
            return categories;
        }

        [AllowAnonymous]
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
                category = p.Category.Category_name,
                image = p.Image,
                price = p.Price,
                stock = p.Stock,
                created_at = p.Created_at,
                username = p.User.Username,
                job_status = p.Job_status.Job_status,
            }).ToList();

            return Ok(productDtos);
        }

        [AllowAnonymous]
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
                    category = p.Category.Category_name,
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
