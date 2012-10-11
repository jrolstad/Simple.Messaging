using RabbitMQ.Client;
using Simple.Messaging.RabbitMq.Factories;
using Simple.Messaging.RabbitMq.Formatters;

namespace Simple.Messaging.RabbitMq
{
    public class RabbitMqMessenger:IMessenger
    {
        private readonly string _uri;
        private readonly IExchangeNameFactory _exchangeNameFactory;
        private readonly IRoutingKeyFactory _routingKeyFactory;
        private readonly IMessageFormatter _messageFormatter;
        private readonly ExchangeConfiguration _configuration;

        public RabbitMqMessenger(string uri, IExchangeNameFactory exchangeNameFactory, IRoutingKeyFactory routingKeyFactory, IMessageFormatter messageFormatter, ExchangeConfiguration configuration)
        {
            _uri = uri;
            _exchangeNameFactory = exchangeNameFactory;
            _routingKeyFactory = routingKeyFactory;
            _messageFormatter = messageFormatter;
            _configuration = configuration;
        }

        public void Dispose()
        {
            // Nothing to see here.  Move along.
        }

        public void Send<T>(T message)
        {
            var connectionFactory = new ConnectionFactory {Uri = _uri};

            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var exchange = _exchangeNameFactory.Build<T>();
                var routingKey = _routingKeyFactory.Build<T>();
                var messageBody = _messageFormatter.Write(message);

                var basicProperties = channel.CreateBasicProperties();
                basicProperties.ContentType = _messageFormatter.GetMIMEContentType();

                channel.ExchangeDeclare(exchange,_configuration.ExchangeType,_configuration.IsExchangeDurable);
               
                channel.BasicPublish(exchange, routingKey, basicProperties, messageBody);

                channel.Close();
                connection.Close();
            }
           
        }
    }
    
}