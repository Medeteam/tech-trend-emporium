using App.Controllers;
using Bogus;
using Data;
using Data.DTOs;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Tests.ControllersTests
{
    public class ProductControllerTest
    {
        private readonly DBContextTechEmporiumTrend _context;
        private readonly ProductController _controller;
        private readonly Faker<Product> _faker;

        public ProductControllerTest()
        {
            var options = new DbContextOptionsBuilder<DBContextTechEmporiumTrend>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

            _context = new DBContextTechEmporiumTrend(options);
            _controller = new ProductController(_context);

            _faker = new Faker<Product>()
                .RuleFor(p => p.Product_id, Guid.NewGuid)
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.Category, f => new Category
                {
                    Category_name = f.Commerce.Categories(1).First(),
                    Category_description = f.Lorem.Text()
                })
                .RuleFor(p => p.Image, f => f.Image.PicsumUrl())
                .RuleFor(p => p.Price, f => f.Random.Decimal(0, 100))
                .RuleFor(p => p.Stock, f => (uint)f.Random.Int(0, 100));
        }

        [Fact]
        public void GetProducts_Successfully()
        {
            // Arrange
            var mockProducts = _faker.Generate(5);

            _context.Products.AddRange(mockProducts);
            _context.SaveChanges();

            // Act
            var result = _controller.GetProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ProductDto>>(okResult.Value);
        }

        [Fact]
        public void GetProductById_Successfully()
        {
            // Arrange
            var mockProduct = _faker.Generate();
            var id = mockProduct.Product_id;

            _context.Products.Add(mockProduct);
            _context.SaveChanges();

            // Act
            var result = _controller.GetProductById(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ProductDto>(okResult.Value);
        }

        [Fact]
        public void GetProductById_UsingWrongId()
        {
            // Arrange
            var mockProduct = _faker.Generate();
            var id = Guid.NewGuid();

            _context.Products.Add(mockProduct);
            _context.SaveChanges();

            // Act
            var result = _controller.GetProductById(id);

            // Assert
            var okResult = Assert.IsType<NotFoundObjectResult>(result);

        }
    }
}
