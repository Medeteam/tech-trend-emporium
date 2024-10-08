using Data.Entities;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Data.DTOs;
using System.Security.Claims;

namespace App.Controllers
{
    [ApiController]
    [Route("/api")]
    public class UserController : ControllerBase
    {
        private readonly DBContextTechEmporiumTrend _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserController(DBContextTechEmporiumTrend context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        // Endpoint to get all users (route: api/user)
        [HttpGet("user")]
        [Authorize(Policy = "RequireAdminRole")]
        public IActionResult GetAllUsers(int pageNumber = 1)
        {
            int pageSize = 5;
            if (pageNumber <= 0) pageNumber = 1;
            var totalUsers = _context.Users.Count();

            var users = _context.Users
                .Select(u => new
                {
                    u.User_id,
                    u.Username,
                    u.Email,
                    Role = u.Role.RoleName
                })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new
            {
                TotalUsers = totalUsers,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Users = users
            };

            return Ok(result);
        }


        // Endpoint to get an user by id (route: api/user/{id})
        [HttpGet("user/{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public IActionResult GetUserById(Guid id)
        {
            var user = _context.Users
                .Where(u => u.User_id == id)
                .Select(u => new
                {
                    u.User_id,
                    u.Username,
                    u.Email,
                    Role = u.Role.RoleName
                })
                .FirstOrDefault();

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }

        // Endpoint to get all users by specific role (route: api/user/role/{rolename})
        [HttpGet("user/role/{roleName}")]
        [Authorize(Policy = "RequireAdminRole")]
        public IActionResult GetUsersByRole(string roleName, int pageNumber = 1)
        {
            int pageSize = 5;

            if (pageNumber <= 0) pageNumber = 1;
            var totalUsers = _context.Users
                .Where(u => u.Role.RoleName == roleName)
                .Count();

            var users = _context.Users
                .Where(u => u.Role.RoleName == roleName)
                .Select(u => new
                {
                    u.User_id,
                    u.Username,
                    u.Email,
                    Role = u.Role.RoleName
                })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            if (!users.Any())
            {
                return NotFound($"Not found users with the role '{roleName}'.");
            }

            var result = new
            {
                TotalUsers = totalUsers,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Users = users
            };

            return Ok(result);
        }

        // Endpoint to delete an users (route: api/user)
        [HttpDelete("user/{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            var user = _context.Users.FirstOrDefault(u => u.User_id == id);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }
            if (userId == user.User_id.ToString()) {
                return Conflict(new { message = "User can not be deleted" });
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User deleted successfully" });
        }

        // Endpoint to modify all users (route: api/user)
        [HttpPut("user")]
        [Authorize("RequireAdminRole")]
        public async Task<IActionResult> UpdateUser(UserDto userDto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == userDto.name);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }
            user.Username = userDto.Username;
            user.Email = userDto.Email;
            user.Password = userDto.Password;
            user.Password = _passwordHasher.HashPassword(user, user.Password);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User updated successfully" });
        }
    }
}

