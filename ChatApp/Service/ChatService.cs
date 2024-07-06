using ChatApp.Hubs;
using ChatApp.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Service
{
    public class ChatService : IChatService
    {
        private readonly ChatContext _context;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatService(ChatContext context, IHubContext<ChatHub> hubContext)
        {
            this._context = context;
            this._hubContext = hubContext;
        }
        public async Task<Chat> CreateChatAsync(Chat chat)
        {
            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();
            return chat;
        }

        public async Task<bool> DeleteChatAsync(int chatId, int userId)
        {
            var chat = await _context.Chats.FindAsync(chatId);

            if (chat != null)
            {
                return false;
            }

            if (chat.CreatedByUserId != userId)
            {
                throw new UnauthorizedAccessException("There are no permissions to do the operation");
            }

            _context.Chats.Remove(chat);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.Group(chatId.ToString()).SendAsync("ChatDeleted", chatId);

            return true;

        }

        public async Task<IEnumerable<Chat>> GetAllChatsAsync()
        {
            return await _context.Chats.ToListAsync();
        }

        public async Task<Chat> GetChatByIdAsync(int chatId)
        {
            return await _context.Chats.FindAsync(chatId);
        }
    }
}
