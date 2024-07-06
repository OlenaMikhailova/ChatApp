using ChatApp.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            this._chatService = chatService;
        }


        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Chat API is running.");
        }
    }
}
