using ChatApp.Models;

namespace ChatApp.Service
{
    public interface IChatService
    {
        Task<IEnumerable<Chat>> GetAllChatsAsync();
        Task<Chat> GetChatByIdAsync(int chatId);
        Task<Chat> CreateChatAsync(Chat chat);
        Task<bool> DeleteChatAsync(int chatId, int userId);
        Task<List<string>> GetUsersInChatAsync(int chatId);
    }
}
