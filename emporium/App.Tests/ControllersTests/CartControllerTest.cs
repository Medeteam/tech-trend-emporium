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
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace App.Tests.ControllersTests
{
    public class CartControllerTest
    {
        private readonly DBContextTechEmporiumTrend _context;
        private readonly CartController _controller;
        private readonly Faker<Cart> _fakerCart;
        private readonly Faker<User> _fakerUser;
        private readonly Faker<Product> _fakerProduct;

        public CartControllerTest()
        {
            var options = new DbContextOptionsBuilder<DBContextTechEmporiumTrend>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

            _context = new DBContextTechEmporiumTrend(options);
            _controller = new CartController(_context);

            _fakerCart = new Faker<Cart>()
                .RuleFor(c => c.Cart_id, c => Guid.NewGuid());
            _fakerUser = new Faker<User>()
                .RuleFor(u => u.User_id, u => Guid.NewGuid())
                .RuleFor(u => u.Username, f => f.Person.UserName)
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Password, f => f.Internet.Password());
            _fakerProduct = new Faker<Product>()
                .RuleFor(p => p.Product_id, p => Guid.NewGuid())
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.Image, f => f.Image.PicsumUrl());
        }

        [Fact]
        public async Task AddProductToCart_Successfully()
        {
            // Arrange
            var user = _fakerUser.Generate();
            var cart = _fakerCart.Generate();
            user.Cart = cart;
            _context.Users.Add(user);
            var product = _fakerProduct.Generate();
            product.Stock = 10;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var userIdClaim = new Claim(ClaimTypes.Sid, user.User_id.ToString());
            var identity = new ClaimsIdentity(new[] { userIdClaim });
            var claimsPrincipal = new ClaimsPrincipal(identity);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            var request = new ProductRequestDto
            {
                productId = product.Product_id,
                quantity = 1
            };

            // Act
            var result = await _controller.AddProductToCart(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }



    }
}
