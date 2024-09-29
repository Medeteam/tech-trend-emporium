using App.Controllers;
using Bogus;
using Data.Entities;
using Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace App.Tests.ControllersTests
{
    public class CategoryControllerTest
    {
        private readonly DBContextTechEmporiumTrend _context;
        private readonly CategoryController _controller;
        private readonly Faker<Category> _faker;

        public CategoryControllerTest()
        {
            var options = new DbContextOptionsBuilder<DBContextTechEmporiumTrend>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

            _context = new DBContextTechEmporiumTrend(options);
            _controller = new CategoryController(_context);

            _faker = new Faker<Category>()
                .RuleFor(c => c.Category_id, f => Guid.NewGuid())
                .RuleFor(c => c.Category_name, f => f.Commerce.Categories(1).First())
                .RuleFor(c => c.Category_description, f => f.Lorem.Text());
        }

        private void ClearContext()
        {
            _context.Categories.RemoveRange(_context.Categories);
            _context.SaveChanges();
        }

        [Fact]
        public void GetCategories_Successfully()
        {
            // Arrange
            var mockCategories = _faker.Generate(5);

            _context.Categories.AddRange(mockCategories);
            _context.SaveChanges();

            // Act
            var result = _controller.GetCategories();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<CategoryDto>>(okResult.Value);
        }

        [Fact]
        public void GetCategoryById_Successfully()
        {
            // Arrange
            var mockCategory = _faker.Generate();
            var id = mockCategory.Category_id;

            _context.Categories.Add(mockCategory);
            _context.SaveChanges();

            // Act
            var result = _controller.GetCategoryById(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CategoryDto>(okResult.Value);
        }

        [Fact]
        public void GetCategoryById_WithWrongId()
        {
            // Arrange
            var mockCategory = _faker.Generate();
            var id = Guid.NewGuid();

            _context.Categories.Add(mockCategory);
            _context.SaveChanges();

            // Act
            var result = _controller.GetCategoryById(id);

            // Assert
            var okResult = Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeleteCategory_Successfully()
        {
            // Arrange
            ClearContext();
            var mockCategory = _faker.Generate();
            var id = mockCategory.Category_id;

            _context.Categories.Add(mockCategory);
            _context.SaveChanges();

            // Act
            var result = await _controller.DeleteCategory(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            var categoriesCount = _context.Categories.Count();
            Assert.True(categoriesCount == 0);
        }

        [Fact]
        public async Task DeleteProduct_UsingWrongId()
        {
            // Arrange
            ClearContext();
            var mockCategory = _faker.Generate();
            var id = Guid.NewGuid();

            _context.Categories.Add(mockCategory);
            _context.SaveChanges();

            // Act
            var result = await _controller.DeleteCategory(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);

            var categoriesCount = _context.Categories.Count();
            Assert.True(categoriesCount > 0);

        }
    }
}
