using Data;
using Data.DTOs;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Supabase.Gotrue;
using System.Collections.Generic;
using System.Net.WebSockets;

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
            // Verificar si el usuario existe
            var existingUser = await _context.Users.Include(w => w.WishList).ThenInclude(pw => pw.ProductWishLists).ThenInclude(p => p.Product).FirstOrDefaultAsync(u => u.User_id == user);

            // Verificar si el producto ya está en la wishlist
            var existingWishListItem = existingUser.WishList.ProductWishLists
                .FirstOrDefault(pw => pw.Product_id == productId);

            if (existingWishListItem != null)
            {
                return Conflict("Product already exists in the wishlist.");
            }

            // Agregar el producto a la wishlist
            //var productWishList = new ProductWishList
            //{
            //    Product_id = productId,
            //    Wishlist_id =
            //};
            var asdas = new WishList
            {
                ProductWishLists = new List<ProductWishList> {
                   new(){
                         Product_id = productId,
                         Wishlist_id = existingUser.WishList.Wishlist_id
                   }

               },
                Created_at = DateTime.Now

            };

            _context.Add(asdas);
            _context.SaveChanges();







            //insert into productwsih, 

            //_context.WishLi



            //_context. 00Add(productWishList);
            //await _context.SaveChangesAsync();

            return Ok(new { Message = "Product added to wishlist successfully." });

        }
        //[HttpGet("api/{user}/wishlist")]
        //public async Task<IActionResult> GetProductsFromWishlist(Guid user)
        //{
        //    var existentUser = await _context.Users.FirstOrDefaultAsync(w => w.User_id == user);
        //    if (existentUser == null)
        //    {
        //        return Conflict("User does not exist");
        //    }
        //    var wishList = await _context.WishList
        //                        .Include(w => w)
        //                        .FirstOrDefaultAsync(w => w.User_id == user);

        //    return Ok(wishList?.Products);
        //}
        //[HttpDelete("/api/{user}/wishlist/remove/{productId}")]
        //public async Task<IActionResult> RemoveProductFromWishList(Guid user, Guid productId)
        //{
        //    var ExistingUser = await _context.Users.Include("W").FirstOrDefaultAsync(w => w.User_id == user);
        //    if (ExistingUser == null)
        //    {
        //        return Conflict("User does not exist");
        //    }
        //    var product = await _context.Products.FirstOrDefaultAsync(p => p.Product_id == productId);
        //    if (product == null)
        //    {
        //        return Conflict("Product does not exist");
        //    }
        //    if (!ExistingUser.WishList.Products.Contains(product))
        //    {
        //        return Conflict("Product does not exist in the Wishlist");
        //    }
        //    ExistingUser.WishList.Products.Remove(product);
        //    await _context.SaveChangesAsync();
        //    return Ok(new { Message = "Product removed successfully", Product = product.Name });
        //}
    }
}