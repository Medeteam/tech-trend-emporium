using App.Services;
using Data;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polly;

namespace App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FakeStoreController : ControllerBase
    {
        private readonly FakeStoreService _fakeStoreService;
        private readonly DBContextTechEmporiumTrend _context;

        public FakeStoreController(FakeStoreService fakeStoreService, DBContextTechEmporiumTrend context)
        {
            _fakeStoreService = fakeStoreService;
            _context = context;
        }

        [HttpPost("sync-categories")]
        [Authorize(Policy = "RequireEmployeeOrSuperiorRole")]
        public async Task<IActionResult> SyncCategories()
        {
            // Obtener las categorías desde la API de FakeStore
            var categoriesFromApi = await _fakeStoreService.GetCategoriesAsync();

            // Buscar al usuario específico "CamiloVelezP"
            var User = _context.Users.FirstOrDefault(u => u.Username == "CamiloVelezP");

            // Obtener las categorías ya existentes en la base de datos
            var existingCategories = _context.Categories.Select(c => c.Category_name).ToList();

            // Filtrar las categorías que aún no existen en la base de datos
            var newCategories = categoriesFromApi
                .Where(apiCategory => !existingCategories.Contains(apiCategory))
                .Select(apiCategory => new Category
                {
                    Category_name = apiCategory,
                    Category_description = $"Categoría agregada desde API, sin descripción adicional.",
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

        [HttpPost("sync-products")]
        [Authorize(Policy = "RequireEmployeeOrSuperiorRole")]
        public async Task<IActionResult> SyncProducts()
        {
            // Obtener los productos desde la Fake Store API
            var productsFromApi = await _fakeStoreService.GetProductsAsync();

            // Buscar al usuario específico "CamiloVelezP"
            var user = _context.Users.FirstOrDefault(u => u.Username == "CamiloVelezP");

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
    }
}
