namespace Simple.Messaging
{
    public class MessageQueueDetail
    {
        public int? MessageCount { get; set; }

        public bool Exists { get; set; }

        public string Uri { get; set; }
    }
}