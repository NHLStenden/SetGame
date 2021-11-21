using Backend.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiKeyAuthentication]
    public class ApiKeyExampleController : ControllerBase
    {
        [HttpGet("Secret")]
        public IActionResult GetSecret()
        {
            return Ok("I have used a api-key");
        }
    }
}