using ChatApp.Hubs;
using ChatApp.Models;
using ChatApp.Service;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;


namespace ChatApp.Tests
{
    public class IntegrationTest
    {
        private DbContextOptions<ChatContext> _options;

        //[SetUp]
        //public void Setup()
        //{
        //    _options = new DbContextOptionsBuilder<ChatContext>()
        //        .UseInMemoryDatabase(databaseName: "Test_MessageService")
        //        .Options;
        //}

        [Test]
        public async Task DeleteMessageAsync_ValidMessage_Success()
        {
            using (var context = new ChatContext(_options))
            {
                var mockHubContext = new Mock<IHubContext<ChatHub>>();
                var messageService = new MessageService(context, mockHubContext.Object);

                var message = new Message { MessageId = 1, ChatId = 1, UserId = 1, Text = "Test Message" };
                await context.Messages.AddAsync(message);
                await context.SaveChangesAsync();

                var result = await messageService.DeleteMessageAsync(message.MessageId, message.UserId);
                var deletedMessage = await context.Messages.FindAsync(message.MessageId);

                Assert.That(result, Is.True);
                Assert.That(deletedMessage, Is.Not.Null);
            }
        }
    }
}
