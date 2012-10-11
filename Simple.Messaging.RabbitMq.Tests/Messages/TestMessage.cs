using System;

namespace Simple.Messaging.RabbitMq.Tests.Messages
{
    public class TestMessage
    {
        public string Identifier { get; set; }

        public string Description { get; set; }

        public DateTime SentAt { get; set; }
    }
}