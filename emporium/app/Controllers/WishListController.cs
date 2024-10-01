using Data;
using Data.DTOs;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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
        [Authorize]
        public async Task<IActionResult> AddProductToWishList(Guid user, Guid productId)
        {
            // Verificar si el usuario existe
            var existingUser = await _context.Users.Include(w => w.WishList).ThenInclude(pw => pw.ProductWishLists).ThenInclude(p => p.Product).FirstOrDefaultAsync(u => u.User_id == user);

            // Verificar si el producto ya está en la wishlist

            var existingWishListItem = existingUser.WishList?.ProductWishLists.FirstOrDefault(pw => pw.Product_id == productId);

            if (existingWishListItem != null)
            {
                return Conflict("Product already exists in the wishlist.");
            }

            // Agregar el producto a la wishlist del usuario
            var newProductWishList = new ProductWishList
            {
                Product_id = productId,
                Wishlist_id = existingUser.WishList.Wishlist_id
            };

            _context.ProductWishLists.Add(newProductWishList);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Product added to wishlist successfully." });

        }
        [HttpGet("api/{user}/wishlist")]
        [Authorize]
        public async Task<IActionResult> GetProductsFromWishlist(Guid user)
        {
            var existingUser = await _context.Users.Include(w => w.WishList).ThenInclude(pw => pw.ProductWishLists).ThenInclude(p => p.Product).FirstOrDefaultAsync(u => u.User_id == user);
            var productIds = existingUser.WishList.ProductWishLists
                .Select(pw => pw.Product.Product_id)
                .ToList();

            return Ok(productIds);
        }

        [HttpDelete("/api/{user}/wishlist/remove/{productId}")]
        [Authorize]
        public async Task<IActionResult> RemoveProductFromWishList(Guid user, Guid productId)
        {
            var existingUser = await _context.Users.Include(w => w.WishList).ThenInclude(pw => pw.ProductWishLists).ThenInclude(p => p.Product).FirstOrDefaultAsync(u => u.User_id == user);

            // Buscar el producto en la wishlist
            var productWishListItem = existingUser.WishList.ProductWishLists
                .FirstOrDefault(pw => pw.Product_id == productId);

            if (productWishListItem == null)
            {
                return NotFound("Product is not in the wishlist.");
            }

            // Eliminar el producto de la wishlist
            _context.ProductWishLists.Remove(productWishListItem);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Product removed from wishlist successfully." });
        }
        }
}