using ChatApp.Models;
using ChatApp.Service;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;
        private readonly IChatService _chatService;

        public ChatHub(IMessageService messageService, IChatService chatService)
        {
            this._messageService = messageService;
            this._chatService = chatService;
        }

        public async Task JoinChat(int chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
            await Clients.Group(chatId.ToString()).SendAsync("UserJoined", Context.ConnectionId);
        }

        public async Task LeaveChat(int chatId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
            await Clients.Group(chatId.ToString()).SendAsync("UserLeft", Context.ConnectionId);
        }

        public async Task DeletedChat(int chatId)
        {
            await Clients.Group(chatId.ToString()).SendAsync("ChatClosed");

            var usersInChat = await _chatService.GetUsersInChatAsync(chatId);

            foreach (var connectionId in usersInChat)
            {
                await Groups.RemoveFromGroupAsync(connectionId, chatId.ToString());
                await Clients.Client(connectionId).SendAsync("Disconnected");
            }
        }

        public async Task SendMessageAsync(int chatId,
                                       string message,
                                       int userId) 
        {
            var chat = await _chatService.GetChatByIdAsync(chatId);
            if (chat == null)
            {
                await Clients.Caller.SendAsync("Error", "Chat does not exist or has been deleted.");
                return;
            }

            var newMessage = new Message
            {
                ChatId = chatId,
                UserId = userId,
                Text = message
            };

            await _messageService.AddMessageAsync(newMessage);

            await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", userId, message);
        }

        public async Task EditMessageAsync(int messageId, string newContent, int userId)
        {
            var message = await _messageService.GetMessageByIdAsync(messageId);
            var chat = await _chatService.GetChatByIdAsync(message.ChatId);

            if (chat == null)
            {
                await Clients.Caller.SendAsync("Error", "Chat does not exist or has been deleted.");
                return;
            }

            var result = await _messageService.EditMessageAsync(messageId, newContent, userId);

            if (result)
            {
                await Clients.Group(message.ChatId.ToString()).SendAsync("EditMessage", messageId, newContent);
            }
        }

        public async Task DeleteMessageAsync(int messageId, int userId)
        {
            var message = await _messageService.GetMessageByIdAsync(messageId);
            var chat = await _chatService.GetChatByIdAsync(message.ChatId);

            if (chat == null)
            {
                await Clients.Caller.SendAsync("Error", "Chat does not exist or has been deleted.");
                return;
            }

            var result = await _messageService.DeleteMessageAsync(messageId, userId);
            if (result)
            {
                await Clients.Group(message.ChatId.ToString()).SendAsync("DeleteMessage", messageId);
            }
        }
    }
}
