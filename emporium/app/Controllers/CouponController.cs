using Data.Entities;
using Data;
using Data.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouponsController : ControllerBase
    {
        private readonly DBContextTechEmporiumTrend _context;

        public CouponsController(DBContextTechEmporiumTrend context)
        {
            _context = context;
        }

        // POST: api/Coupons
        [HttpPost]
        [EnableCors("AllowAll")]
        [Authorize(Policy = "RequireEmployeeOrSuperiorRole")]
        public async Task<IActionResult> CreateCoupon([FromBody] CreateCouponDto couponDto)
        {
            // Validación si el código ya existe
            var existingCoupon = await _context.Coupons.FirstOrDefaultAsync(c => c.Code == couponDto.Code);
            if (existingCoupon != null)
            {
                return BadRequest("A coupon with the same code already exists.");
            }

            // Creación del nuevo cupón
            var newCoupon = new Coupon
            {
                Coupon_name = couponDto.Coupon_name,
                Discount = couponDto.Discount,
                Code = couponDto.Code,
                Coupon_status = couponDto.Coupon_status,
                Created_at = DateTimeOffset.Now
            };

            // Agregar el cupón a la base de datos
            _context.Coupons.Add(newCoupon);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Coupon created successfully",
                Coupon = newCoupon
            });
        }
    }
}
