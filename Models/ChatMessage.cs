using System;

namespace MAVE.Models;

public partial class ChatMessage
{
    public int MessageId { get; set; }

    public int SenderId { get; set; }

    public int ReceiverId { get; set; }

    public string Text { get; set; } = null!;

    public DateTime Date { get; set; }

    public bool Read { get; set; }

    public virtual User Sender { get; set; } = null!;

    public virtual User Receiver { get; set; } = null!;
}
