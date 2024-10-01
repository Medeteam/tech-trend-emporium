using Data;
using Data.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace App.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly DBContextTechEmporiumTrend _context;

        public CartController(DBContextTechEmporiumTrend context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("/cart")]
        [Authorize]
        public IActionResult GetCart()
        {
            var userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            var cartDetails = _context.Users
                .Where(user => user.User_id == Guid.Parse(userId)) // Replace userId with the specific user's ID
                .Select(user => new
                {
                    UserId = user.User_id,
                    CartId = user.Cart.Cart_id,
                    Products = user.Cart.ProductsToCart
                        .Select(ptc => new
                        {
                            ProductName = ptc.Product.Name,
                            ProductPrice = ptc.Product.Price
                        })
                })
                .ToList();

            //TODO Create cart DTO and return it
            return Ok();
        }

        [HttpPost]
        [Route("/cart")]
        [Authorize]
        public IActionResult AddProductToCart([FromBody] string productId)
        {
            var userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            var product = _context.Products.Select(p => new ProductDto
                {
                    id = p.Product_id,
                    title = p.Name,
                    price = p.Price,
                    category = p.CategoryName
                })
                .Where(p => p.id == Guid.Parse(userId));

            if (!product.Any())
            {
                return NotFound(new { message = "Product not found" });
            }
            return Ok(new { message = "Product added successfully" });
        }

        [HttpPost]
        [Route("/cart/coupon")]
        [Authorize]
        public IActionResult ApplyCoupon([FromBody] string code)
        {
            var userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            var coupon = _context.Coupons.Select(c => c)
                .Where(c => c.Code == code);

            if (!coupon.Any())
            {
                return NotFound(new { message = "The coupon doesn't exists" });
            }
            //TODO logica de agregar cupon y sumar descuento

            return Ok(new { message = "Coupon applied successfully" });
        }
    }
}
