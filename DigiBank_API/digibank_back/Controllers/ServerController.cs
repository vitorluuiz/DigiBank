using Microsoft.AspNetCore.Mvc;

namespace digibank_back.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ServerController : Controller
    {
        [HttpGet("status")]
        public IActionResult Ping()
        {
            return Ok();
        }
    }
}
