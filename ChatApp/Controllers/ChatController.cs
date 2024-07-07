using ChatApp.Models;
using ChatApp.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;

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

        //GET: api/chat
        //[HttpGet]
        //public IActionResult Get()
        //{
        //    return Ok("Chat API is running.");
        //}

        //GET: api/chat
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Chat>>> GetChats()
        {
            var chats = await _chatService.GetAllChatsAsync();
            return Ok(chats);
        }

        // GET: api/chat/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Chat>> GetChat(int id)
        {
            var chat = await _chatService.GetChatByIdAsync(id);
            if (chat == null)
            {
                return NotFound();
            }

            return Ok(chat);
        }

        // Post: api/chat
        [HttpPost]
        public async Task<ActionResult<Chat>> PostChat(Chat chat)
        {
            var createdChat = await _chatService.CreateChatAsync(chat);
            return CreatedAtAction(nameof(GetChat),new { id =createdChat.ChatId }, createdChat);
        }

        // DELETE: api/chat/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChat(int id, [FromQuery] int userId)
        {
            try
            {
                var result = await _chatService.DeleteChatAsync(id, userId);
                if(!result)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }
    }
}
