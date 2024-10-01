using Data;
using Data.DTOs;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace YourNamespace.Controllers
{
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly DBContextTechEmporiumTrend _context;

        public ReviewsController(DBContextTechEmporiumTrend context)
        {
            _context = context;
        }
        [Route("api/store/products/{product_id}/reviews/add")]
        [HttpPost]
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
            return Ok("Review added to product successfully");
        }

        [Route("api/store/products/{product_id}/reviews")]
        [HttpGet]
        public async Task<IActionResult> GetProductReviews(Guid product_id)
        {
            var product = await _context.Products.Include(p => p.Reviews).ThenInclude(r => r.User).FirstOrDefaultAsync(p => p.Product_id == product_id);

            if (product == null)
            {
                return NotFound("Producto no encontrado.");
            }

            var reviews = product.Reviews.Select(r => new
            {
                r.Review_id,
                r.Review_content,
                r.Review_rate,
                User = r.User.Username
            });

            return Ok(reviews);
        }
    }
}
