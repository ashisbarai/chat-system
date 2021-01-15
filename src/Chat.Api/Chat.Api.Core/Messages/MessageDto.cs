namespace Chat.Api.Core.Messages
{
    public class MessageDto
    {
        public int Sender { get; set; }
        public int Receiver { get; set; }
        public string Message { get; set; }
    }
}
