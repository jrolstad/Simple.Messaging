namespace Simple.Messaging.RabbitMq
{
    public class ExchangeConfiguration
    {
        public string ExchangeType { get; set; }

        public bool IsExchangeDurable { get; set; }
    }
}