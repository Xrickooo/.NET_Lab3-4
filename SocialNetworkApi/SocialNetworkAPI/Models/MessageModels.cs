namespace Lab4.Models;

public class MessageModel
{
    public string Content { get; set; }
}

public class Message
{
    public string Content { get; set; }
    public DateTime SentDate { get; set; }
    public string SenderLogin { get; set; } 
}
