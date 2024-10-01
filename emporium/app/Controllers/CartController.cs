using Data;
using Data.DTOs;
using Data.Entities;
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
        public async Task<IActionResult> GetCart([FromQuery] bool details = false)
        {
            var userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            //var cartDetails = _context.Users
            //    .Where(user => user.User_id == Guid.Parse(userId)) 
            //    .Select(user => new
            //    {
            //        UserId = user.User_id,
            //        CartId = user.Cart.Cart_id,
            //        Products = user.Cart.ProductToCart
            //            .Select(ptc => new
            //            {
            //                ProductName = ptc.Product.Name,
            //                ProductPrice = ptc.Product.Price
            //            })
            //    })
            //    .ToList();
            var cartInformation = await _context.Users
                .Where(user => user.User_id == Guid.Parse(userId))
                .Include(u => u.Cart)
                .ThenInclude(c => c.ProductToCart)
                .ThenInclude(ptc => ptc.Product)
                .ToListAsync();
            if (details)
            {
                var cartDetails = cartInformation.Select(x => new CartDto
                {
                    cartId = x.Cart.Cart_id,
                    userId = x.User_id,
                    products = x.Cart.ProductToCart.Select(ptc => new ProductDto
                    {
                        id = ptc.Product_id,
                        title = ptc.Product.Name,
                        price = ptc.Product.Price,
                        image = ptc.Product.Image,
                    }).ToList(),
                });

                return Ok(cartDetails);
            }
            return Ok();
            //else
            //{
            //    var cartDetails = cartInformation.Select(x => new CartDto { 
            //        cartId = x.Cart.Cart_id
            //        userId = x.User_id,
            //        totalBeforeDiscount = x.Cart.ProductToCart.Pro
            //    })
            //}

        }

        [HttpPost]
        [Route("/cart")]
        [Authorize]
        public async Task<IActionResult> AddProductToCart([FromBody] ProductRequestDto req)
        {
            var userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            var user = await _context.Users
                .Where(u => u.User_id == Guid.Parse(userId))
                .Include(u => u.Cart)
                .FirstAsync();
            var product = _context.Products
                .Where(p => p.Product_id == req.productId)
                .FirstOrDefault();

            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }
            if (req.quantity <= 0 || req.quantity > product.Stock) {
                return Conflict(new { message = "There is not enough stock of this product" });
            }
            var productToAdd = new ProductToCart
            {
                Cart_id = user.Cart.Cart_id,
                Product_id = req.productId,
                Quantity = req.quantity
            };

            _context.Add(productToAdd);
            _context.SaveChanges();

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

        [HttpDelete]
        [Route("/cart")]
        [Authorize]
        public async Task<IActionResult> DeleteProductFromCart([FromBody] Guid productId)
        {
            var a = await _context.Carts.ToListAsync();
            return NotFound();
        }

        [HttpPut]
        [Route("/cart")]
        [Authorize]
        public async Task<IActionResult> ChangeProductQuantity([FromBody] ProductRequestDto req)
        {
            if(req.quantity <= 0)
            {
                return Conflict(new { message = "Invalid quantity" });
            }
            var userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            var user = await _context.Users
                .Where(u => u.User_id == Guid.Parse(userId))
                .Include(u => u.Cart)
                .FirstAsync();

            var product = await _context.ProductsToCart
                .Where(ptc => ptc.Cart_id == user.Cart.Cart_id)
                .Where(ptc => ptc.Product_id == req.productId)
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound(new { message = "Product not found in cart" });
            }

            // TODO: Add check if req.Quantity > product.stock

            product.Quantity = req.quantity;

            _context.ProductsToCart.Update(product);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Product quantity updated successfully" });
        }
    }
}
