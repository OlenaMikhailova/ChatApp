using ChatApp.Models;
using ChatApp.Service;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            this._messageService = messageService;
        }

        //GET: api/message
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Chat>>> GetMessages(int chatId)
        {
            var messages = await _messageService.GetMessageByIdAsync(chatId);
            return Ok(messages);
        }

        // POST: api/message
        [HttpPost]
        public async Task<ActionResult<Message>> PostMessage([FromBody] Message message)
        {
            var createdMessage = await _messageService.AddMessageAsync(message);
            return CreatedAtAction(nameof(GetMessage), new { id = createdMessage.MessageId }, createdMessage);
        }

        // GET: api/message/{id}
        [HttpGet("single/{id}")]
        public async Task<ActionResult<Message>> GetMessage(int id)
        {
            var message = await _messageService.GetMessageByIdAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            return Ok(message);
        }

        // PUT: api/message/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> EditMessage(int id, [FromBody] Message updatedMessage)
        {
            try
            {
                var result = await _messageService.EditMessageAsync(id, updatedMessage.Text, updatedMessage.UserId);
                if (!result)
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

        // DELETE: api/message/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id, [FromQuery] int userId)
        {
            try
            {
                var result = await _messageService.DeleteMessageAsync(id, userId);
                if (!result)
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
