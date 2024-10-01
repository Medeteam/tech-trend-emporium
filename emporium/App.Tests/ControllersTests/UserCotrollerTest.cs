using App.Controllers;
using Bogus;
using Data.Entities;
using Data;
using Data.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace App.Tests.ControllersTests
{
    public class UserControllerTest
    {
        private readonly DBContextTechEmporiumTrend _context;
        private readonly UserController _controller;
        private readonly Faker<User> _faker;

        public UserControllerTest()
        {
            var options = new DbContextOptionsBuilder<DBContextTechEmporiumTrend>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new DBContextTechEmporiumTrend(options);
            _controller = new UserController(_context);

            // Utilizando Bogus para generar datos falsos de usuario
            _faker = new Faker<User>()
                .RuleFor(u => u.User_id, f => Guid.NewGuid())
                .RuleFor(u => u.Username, f => f.Internet.UserName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Password, f => f.Internet.Password())
                .RuleFor(u => u.Role, f => new Role { RoleName = f.PickRandom(new[] { "Admin", "User" }) });
        }

        private void ClearContext()
        {
            _context.Users.RemoveRange(_context.Users);
            _context.SaveChanges();
        }

        [Fact]
        public async Task DeleteUser_Successfully()
        {
            // Arrange
            ClearContext();
            var mockUser = _faker.Generate();
            var id = mockUser.User_id;

            _context.Users.Add(mockUser);
            _context.SaveChanges();
            var usersBeforeDelete = _context.Users.Count();

            // Act
            var result = await _controller.DeleteUser(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var usersAfterDelete = _context.Users.Count();
            Assert.True(usersAfterDelete < usersBeforeDelete);
        }

        [Fact]
        public async Task DeleteUser_WithWrongId()
        {
            // Arrange
            ClearContext();
            var mockUser = _faker.Generate();
            var id = Guid.NewGuid(); // ID que no existe en la base de datos

            _context.Users.Add(mockUser);
            _context.SaveChanges();

            // Act
            var result = await _controller.DeleteUser(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task UpdateUser_Successfully()
        {
            // Arrange
            ClearContext();
            var mockUser = _faker.Generate();
            _context.Users.Add(mockUser);
            _context.SaveChanges();

            var userDto = new UserDto
            {
                name = mockUser.Username,
                Username = "UpdatedUsername",
                Email = "updated@example.com",
                Password = "newpassword123"
            };

            // Act
            var result = await _controller.UpdateUser(userDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            var updatedUser = _context.Users.FirstOrDefault(u => u.User_id == mockUser.User_id);
            Assert.NotNull(updatedUser);
            Assert.Equal("UpdatedUsername", updatedUser.Username);
            Assert.Equal("updated@example.com", updatedUser.Email);
        }
    }
}
