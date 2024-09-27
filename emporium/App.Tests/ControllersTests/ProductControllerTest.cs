using App.Controllers;
using App.Services;
using Bogus;
using Data;
using Data.DTOs;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Polly;

namespace App.Tests.ControllersTests
{
    public class ProductControllerTest
    {
        private readonly FakeStoreService _storeService;
        private readonly DBContextTechEmporiumTrend _context;
        private readonly ProductController _controller;

        public ProductControllerTest()
        {
            _storeService = Substitute.For<FakeStoreService>();
            _context = Substitute.For<DBContextTechEmporiumTrend>();
            _controller = new ProductController(_storeService, _context);
        }
        [Fact]
        public void GetProducts_Successfully()
        {
            // Arrange
            var faker = new Faker<Product>()
                .RuleFor(p => p.Product_id, Guid.NewGuid)
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.CategoryName, f => f.Commerce.Categories(0).First())
                .RuleFor(p => p.Image, f => f.Image.PicsumUrl())
                .RuleFor(p => p.Price, f => f.Random.Decimal(0, 100))
                .RuleFor(p => p.Stock, f => (uint)f.Random.Int(0, 100));

            var mockProducts = faker.Generate(5).AsQueryable();
            _context.Products.Returns(mockProducts);


            // Act
            var result = _controller.GetProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ProductDto>>(okResult.Value);
            Assert.Equal(5, returnValue.Count);
        }
    }
}
