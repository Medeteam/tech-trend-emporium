using Data;
using Data.DTOs;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("/api")]
    public class ReviewsController : ControllerBase
    {
        private readonly DBContextTechEmporiumTrend _context;

        public ReviewsController(DBContextTechEmporiumTrend context)
        {
            _context = context;
        }

        // Endpoint add the review for a product (route: api/store/products/{product_id}/reviews/add)
        [Route("store/products/{product_id}/reviews/add")]
        [HttpPost]
        [EnableCors("AllowAll")]
        [Authorize]
        public async Task<IActionResult> AddReview(Guid product_id,[FromBody] ReviewRequestDto reviewDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(c => c.Username == reviewDto.User ); 
            var newReview = new Review
            {
                User = user,
                Review_content = reviewDto.Comment,
                Review_rate = reviewDto.Rate,
                Product_id = product_id 
            };

            _context.Reviews.Add(newReview);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Review added to product successfully" });
        }

        // Endpoint to get the reviews of a product (route: api/store/products/{product_id}/reviews)
        [Route("store/products/{product_id}/reviews")]
        [HttpGet]
        public async Task<IActionResult> GetProductReviews(Guid product_id)
        {
            if (!_context.Products.Any(p => p.Product_id == product_id))
            {
                return NotFound(new { message = "Productnot found" });
            }
            var reviews = await _context.Reviews
                .Where(r => r.Product_id == product_id)
                .Include(r => r.User)
                .Select(r => new ReviewDto
                {
                    user = r.User.Username,
                    productId = product_id,
                    rate = r.Review_rate ?? 0,
                    comment = r.Review_content
                })
                .ToListAsync();

            return Ok(reviews);
        }

        // Endpoint to delete the review of a product (route: api/store/products/{product_id}/reviews/remove)
        [Route("store/products/{product_id}/reviews/remove")]
        [HttpDelete]
        [EnableCors("AllowAll")]
        [Authorize]
        public async Task<IActionResult> DeleteReview(Guid product_id)
        {
            var userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            var review = await _context.Reviews
                .Where(r => r.User_id == Guid.Parse(userId))
                .Where(r => r.Product_id == product_id)
                .FirstOrDefaultAsync();

            if (review == null)
            {
                return NotFound(new { message = "Review not found" });
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Review deleted successfully" });
        }
    }
}
