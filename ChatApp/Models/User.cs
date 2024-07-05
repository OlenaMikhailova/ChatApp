using System;

namespace ChatApp.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public ICollection<Chat> Chats { get; set; }
    }
}
