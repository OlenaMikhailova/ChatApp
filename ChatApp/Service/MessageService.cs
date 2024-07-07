using ChatApp.Hubs;
using ChatApp.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;

namespace ChatApp.Service
{
    public class MessageService : IMessageService
    {
        private readonly ChatContext _context;
        private readonly IHubContext<ChatHub> _hubContext;

        public MessageService(ChatContext context, IHubContext<ChatHub> hubContext)
        {
            this._context = context;
            this._hubContext = hubContext;
        }
        public async Task<Message> AddMessageAsync(Message message)
        {
            var chat = await _context.Chats.FindAsync(message.ChatId);

            if (chat == null)
            {
                throw new ArgumentException("Chat not found");
            }

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.Group(message.ChatId.ToString()).SendAsync("ReceiveMessage", message);
            return message;
        }

        public async Task<bool> EditMessageAsync(int messageId, string newText, int userId)
        {
            var message = await _context.Messages.FindAsync(messageId);

            if (message == null)
            {
                return false;
            }

            if (message.UserId != userId) 
            {
                throw new UnauthorizedAccessException("There are no permissions to do the operation");
            }

            message.Text = newText;
            await _context.SaveChangesAsync();

            await _hubContext.Clients.Group(message.ChatId.ToString()).SendAsync("EditMessage", message);

            return true;
        }

        public async Task<bool> DeleteMessageAsync(int messageId, int userId)
        {
            var message = await _context.Messages.FindAsync(messageId);

            if (message == null)
            {
                return false;
            }

            if (message.UserId != userId)
            {
                throw new UnauthorizedAccessException("There are no permissions to do the operation");
            }

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.Group(message.ChatId.ToString()).SendAsync("DeleteMessage", messageId);

            return true;
        }

        public async Task<Message> GetMessageByIdAsync(int messageId)
        {
            return await _context.Messages.FindAsync(messageId);
        }

    }
}
