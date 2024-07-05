namespace ChatApp.Models
{
    public class Chat
    {
        public int ChatId { get; set; }
        public string ChatName { get; set; }
        public int CreatedByUserId { get; set; }
        public User CreatedBy { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
