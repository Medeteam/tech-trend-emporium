﻿using Data;
using Data.DTOs;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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
            var userCartInformation = await _context.Users
                .Where(user => user.User_id == Guid.Parse(userId))
                .Include(u => u.Cart)
                .ThenInclude(c => c.ProductToCart)
                .ThenInclude(ptc => ptc.Product)
                .ToListAsync();

            var cartId = userCartInformation.Select(u => u.Cart.Cart_id).FirstOrDefault();
            var totalBeforeDiscount = await GetCartPrice(cartId);
            var coupon = userCartInformation.Select(u => u.Cart.Coupon).FirstOrDefault();
            var totalAfterDiscount = totalBeforeDiscount;
            if (coupon != null) {
                totalAfterDiscount = GetDiscountedPrice(totalBeforeDiscount, coupon.Discount);
            }

            var cartGeneralInfo = userCartInformation.FirstOrDefault();
            var cartDetails = new CartDto
            {
                cartId = cartId,
                userId = cartGeneralInfo.User_id,
                totalBeforeDiscount = totalBeforeDiscount,
                totalAfterDiscount = totalAfterDiscount,
                shippingCost = 5.00m,
                finalTotal = totalAfterDiscount + 5
            };

            if (details)
            {
                var products = userCartInformation.SelectMany(x => x.Cart.ProductToCart)
                    .Select(ptc => new ProductCartDto
                    {
                        id = ptc.Product_id,
                        title = ptc.Product.Name,
                        price = ptc.Product.Price,
                        image = ptc.Product.Image,
                        quantity = ptc.Quantity
                    }).ToList();
                cartDetails.products = products;
            }

            return Ok(cartDetails);

        }

        private async Task<decimal> GetCartPrice(Guid cartId)
        {
            var products = await _context.ProductsToCart
                .Where(ptc => ptc.Cart_id == cartId)
                .Include(ptc => ptc.Product)
                .ToListAsync();
            decimal total = 0;
            foreach (var product in products)
            {
                total += product.Quantity * product.Product.Price;
            }
            return total;
        }

        private decimal GetDiscountedPrice(decimal price, int discount)
        {
            decimal discounted = price * discount / 100;
            return (price - discounted);
        }

        [HttpPost]
        [Route("/cart")]
        [Authorize]
        public async Task<IActionResult> AddProductToCart([FromBody] ProductRequestDto request)
        {
            var userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            var user = await _context.Users
                .Where(u => u.User_id == Guid.Parse(userId))
                .Include(u => u.Cart)
                .FirstAsync();
            var product = _context.Products
                .Where(p => p.Product_id == request.productId)
                .FirstOrDefault();

            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }
            if (request.quantity <= 0 || request.quantity > product.Stock) {
                return Conflict(new { message = "There is not enough stock of this product" });
            }
            var productToAdd = new ProductToCart
            {
                Cart_id = user.Cart.Cart_id,
                Product_id = request.productId,
                Quantity = request.quantity
            };

            _context.Add(productToAdd);
            _context.SaveChanges();

            return Ok(new { message = "Product added successfully" });
        }

        [HttpDelete]
        [Route("/cart")]
        [Authorize]
        public async Task<IActionResult> DeleteProductFromCart([FromBody] DeleteProductRequestDto request)
        {
            var userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            var userCart = _context.Users
                .Where(u => u.User_id == Guid.Parse(userId))
                .Include(u => u.Cart)
                .First();
            var product = _context.ProductsToCart
                .Where(ptc => ptc.Product_id == request.productId)
                .Where(ptc => ptc.Cart_id == userCart.Cart.Cart_id)
                .FirstOrDefault();

            if (product == null)
            {
                return NotFound(new { message = "This product is not added to the cart" });
            }

            _context.Remove(product);
            _context.SaveChanges();
            return Ok(new { message = "Product removed from the cart" });
        }

        [HttpPut]
        [Route("/cart")]
        [Authorize]
        public async Task<IActionResult> ChangeProductQuantity([FromBody] ProductRequestDto request)
        {
            if (request.quantity <= 0)
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
                .Where(ptc => ptc.Product_id == request.productId)
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound(new { message = "Product not found in cart" });
            }
            if (product.Product.Stock < request.quantity)
            {
                return Conflict(new { message = "There is not enough stock for this item" });
            }

            product.Quantity = request.quantity;

            _context.ProductsToCart.Update(product);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Product quantity updated successfully" });
        }

        [HttpPost]
        [Route("/cart/coupon")]
        [Authorize]
        public async Task<IActionResult> ApplyCoupon([FromBody] CouponRequestDto request)
        {
            var userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            var coupon = _context.Coupons
                .Where(c => c.Code == request.code)
                .FirstOrDefault();

            if (coupon == null)
            {
                return NotFound(new { message = "The coupon doesn't exists" });
            }
            if(coupon.Coupon_status == false)
            {
                return Conflict(new { message = "The coupon is not valid" });
            }
            //TODO logica de agregar cupon y sumar descuento
            var userCart = _context.Users
                .Where(u => u.User_id == Guid.Parse(userId))
                .Include(u => u.Cart)
                .First();
            userCart.Cart.Coupon = coupon;
            //    new Coupon
            //{
            //    Coupon_name = coupon.Coupon_name,
            //    Code = coupon.Code,
            //    Coupon_id = coupon.Coupon_id,
            //};
            //var cart = new Cart
            //{
            //    Cart_id = userCart.Cart.Cart_id,
            //    Coupon = new Coupon
            //    {
            //        Coupon_name = coupon.Coupon_name,
            //        Code = coupon.Code,
            //        Coupon_id = coupon.Coupon_id,
            //    }
            //};

            _context.Update(userCart);
            _context.SaveChanges();

            return Ok(new { message = "Coupon applied successfully" });
        }
    }
}
