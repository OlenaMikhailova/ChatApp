using ChatApp.Hubs;
using ChatApp.Models;
using ChatApp.Service;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using Moq;
using NUnit.Framework;

namespace ChatApp.Tests
{
    public class UnitTests
    {
        [Test]
        public async Task CreateChatAsync_ValidChat_ReturnsChat()
        {
            var mockContext = new Mock<ChatContext>();
            var mockHubContext = new Mock<IHubContext<ChatHub>>();

            var chatService = new ChatService(mockContext.Object, mockHubContext.Object);
            var chat = new Chat { ChatId = 1, ChatName = "Test Chat", CreatedByUserId = 1 };

            mockContext.Setup(m => m.SaveChangesAsync(default)).ReturnsAsync(1);

            var result = await chatService.CreateChatAsync(chat);

            Assert.Equals(chat, result);
        }
    }
}
