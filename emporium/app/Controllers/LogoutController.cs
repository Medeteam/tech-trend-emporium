using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogoutController : ControllerBase
    {
        [HttpPost]
        [Authorize]
        public IActionResult Logout()
        {
            if (Request.Cookies.ContainsKey("Authorization"))
            {
                Response.Cookies.Delete("Authorization");
                return Ok(new { message = "Logged out successfully" });
            }
            else
            {
                return Ok(new { message = "User is not logged" });
            }
        }
    }
}
