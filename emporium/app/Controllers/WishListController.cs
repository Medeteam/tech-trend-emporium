using Data;
using Data.DTOs;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace App.Controllers
{
    [ApiController]
    [Route("/api")]
    public class WishListController : ControllerBase
    {
        private readonly DBContextTechEmporiumTrend _context;

        public WishListController(DBContextTechEmporiumTrend context)
        {
            _context = context;
        }

        // Endpoint to add a product to wishlist (route: api/{user}/wishlist/add/{productId})
        [HttpPost("{user}/wishlist/add/{productId}")]
        [Authorize]
        public async Task<IActionResult> AddProductToWishList(Guid user, Guid productId)
        {
            if(!_context.Products.Any(p => p.Product_id == productId))
            {
                return NotFound(new { message = "Product not found" });
            }

            // Verify that user exists
            var userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            var existingUser = await _context.Users
                .Include(u => u.WishList)
                .ThenInclude(w => w.ProductWishLists)
                .ThenInclude(pw => pw.Product)
                .FirstOrDefaultAsync(u => u.User_id == user);
            if (existingUser == null || user != Guid.Parse(userId))
            {
                return Forbid();
            }

            // Verify if product is alredy in wishlist

            var existingWishListItem = existingUser.WishList?.ProductWishLists.FirstOrDefault(pw => pw.Product_id == productId);

            if (existingWishListItem != null)
            {
                return Conflict(new { message = "Product already exists in the wishlist." });
            }

            // Add product to wishlist
            var newProductWishList = new ProductWishList
            {
                Product_id = productId,
                Wishlist_id = existingUser.WishList.Wishlist_id
            };

            _context.ProductWishLists.Add(newProductWishList);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Product added to wishlist successfully." });

        }

        // Endpoint to get user's wishlist (route: api/{user}/wishlist)
        [HttpGet("{user}/wishlist")]
        public async Task<IActionResult> GetProductsFromWishlist(Guid user)
        {
            var existingUser = await _context.Users.Include(w => w.WishList).ThenInclude(pw => pw.ProductWishLists).ThenInclude(p => p.Product).FirstOrDefaultAsync(u => u.User_id == user);
            if(existingUser == null)
            {
                return NotFound(new { message = "User not found" });
            }
            
            var productIds = existingUser.WishList.ProductWishLists
                .Select(pw => pw.Product.Product_id)
                .ToList();
            var wishlist = new WishListDto
            {
                userId = user,
                productList = productIds
            };

            return Ok(wishlist);
        }

        // Endpoint to delete a product from wishlist (route: api/{user}/wishlist/remove/{productId})
        [HttpDelete("{user}/wishlist/remove/{productId}")]
        [Authorize]
        public async Task<IActionResult> RemoveProductFromWishList(Guid user, Guid productId)
        {
            var userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            var existingUser = await _context.Users
                .Include(u => u.WishList)
                .ThenInclude(w => w.ProductWishLists)
                .ThenInclude(wp => wp.Product)
                .FirstOrDefaultAsync(u => u.User_id == user);
            if (existingUser == null || user != Guid.Parse(userId))
            {
                return Forbid();
            }

            // Search product in wishlist
            var productWishListItem = existingUser.WishList.ProductWishLists
                .FirstOrDefault(pw => pw.Product_id == productId);

            if (productWishListItem == null)
            {
                return NotFound("Product is not in the wishlist.");
            }

            // Delete product from wishlist
            _context.ProductWishLists.Remove(productWishListItem);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Product removed from wishlist successfully." });
        }
    }
}