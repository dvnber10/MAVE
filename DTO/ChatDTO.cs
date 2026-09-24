namespace MAVE.DTO
{
    public class ChatMessageDTO
    {
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public bool Read { get; set; }
        public string SenderName { get; set; } = string.Empty;
    }

    public class ConversationDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string LastText { get; set; } = string.Empty;
        public DateTime LastDate { get; set; }
        public int Unread { get; set; }
    }
}
