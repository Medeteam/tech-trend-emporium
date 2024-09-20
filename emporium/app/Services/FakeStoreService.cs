using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Data.Entities;

namespace App.Services
{
    public class FakeStoreService
    {
        private readonly HttpClient _httpClient;

        public FakeStoreService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Obtener productos directamente dela API
        public async Task<List<Product>> GetProductsAsync()
        {
            var response = await _httpClient.GetAsync("https://fakestoreapi.com/products");
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Asegúrate de que el deserializador utilice el modelo correcto con los atributos JsonPropertyName
            var allProducts = JsonSerializer.Deserialize<List<Product>>(jsonResponse);

            return allProducts;
        }

        //Obtener categorias directamente de la API
        public async Task<List<string>> GetCategoriesAsync()
        {
            var response = await _httpClient.GetAsync("https://fakestoreapi.com/products/categories");
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Deserializar la respuesta JSON en una lista de cadenas (categorías)
            var categories = JsonSerializer.Deserialize<List<string>>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            return categories;
        }

    }
}
