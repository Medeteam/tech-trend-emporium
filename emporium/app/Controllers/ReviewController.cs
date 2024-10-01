using Data;
using Data.DTOs;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
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
    }
}
