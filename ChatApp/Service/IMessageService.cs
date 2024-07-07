using ChatApp.Models;

namespace ChatApp.Service
{
    public interface IMessageService
    {
        Task<Message> AddMessageAsync(Message message);
        Task<bool> EditMessageAsync(int messageId, string newContent, int userId);
        Task<bool> DeleteMessageAsync(int messageId, int userId);
        Task<Message> GetMessageByIdAsync(int messageId);
    }
}
