using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        // GET: api/<ProductsManagementController>
        [HttpGet]
        public IActionResult Product()
        {
            //TODO
            throw new NotImplementedException();
        }

        // GET api/<ProductsManagementController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ProductsManagementController>
        [HttpPost]
        [Authorize(Policy = "RequireEmployeeOrSuperiorRole")]
        public IActionResult Product([FromBody] object placeholder)
        {
            //TODO
            return Ok("This page is visible to Employees and Admins");
        }

        // PUT api/<ProductsManagementController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProductsManagementController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
