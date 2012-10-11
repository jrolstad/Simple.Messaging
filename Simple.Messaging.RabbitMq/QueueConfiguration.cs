namespace Simple.Messaging.RabbitMq
{
    public class QueueConfiguration
    {
        public bool IsQueueDurable { get; set; }

        public bool IsQueueExclusive { get; set; }

        public bool SupportsAutoDelete { get; set; }
    }
}