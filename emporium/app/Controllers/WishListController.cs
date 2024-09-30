using Data;
using Data.DTOs;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Supabase.Gotrue;

namespace App.Controllers
{
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly DBContextTechEmporiumTrend _context;
        
        public WishListController(DBContextTechEmporiumTrend context)
        {
            _context = context; 
        }
        [HttpPost("/api/{user}/wishlist/add/{productId}")]
        public async Task<IActionResult> AddProductToWishList(Guid user, Guid productId)
        {
            var existingUser = await _context.Users.Include("WishList").FirstOrDefaultAsync(w => w.User_id == user);
            if (existingUser == null)
            {
                return Conflict("User does not exist");
            }
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Product_id == productId);
            if (product == null)
            {
                return Conflict("Product does not exist");
            }
            if (existingUser.WishList.Products.Contains(product))
            {
                return Conflict("Product Already exist on Wishlist");
            }
            existingUser.WishList.Products.Add(product);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Product Added Successfully", Product = product.Name });

        }
        [HttpGet("api/{user}/wishlist")]
        public async Task<IActionResult> GetProductsFromWishlist(Guid user)
        {
            var existentUser = await _context.Users.FirstOrDefaultAsync(w => w.User_id == user);
            if (existentUser == null)
            {
                return Conflict("User does not exist");
            }
            var wishList = await _context.WishList
                                .Include(w => w.Products)
                                .FirstOrDefaultAsync(w => w.User_id == user);

            return Ok(wishList?.Products);
        }
        [HttpDelete("/api/{user}/wishlist/remove/{productId}")]
        public async Task<IActionResult> RemoveProductFromWishList(Guid user, Guid productId)
        {
            var ExistingUser = await _context.Users.Include("W").FirstOrDefaultAsync(w => w.User_id == user);
            if (ExistingUser == null)
            {
                return Conflict("User does not exist");
            }
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Product_id == productId);
            if (product == null)
            {
                return Conflict("Product does not exist");
            }
            if (!ExistingUser.WishList.Products.Contains(product))
            {
                return Conflict("Product does not exist in the Wishlist");
            }
            ExistingUser.WishList.Products.Remove(product);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Product removed successfully", Product = product.Name });
        }
    }
}
