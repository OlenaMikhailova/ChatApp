using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Chat API is running.");
        }
    }
}
