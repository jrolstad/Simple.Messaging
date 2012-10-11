using System;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using Simple.Messaging.RabbitMq.Factories;
using Simple.Messaging.RabbitMq.Formatters;

namespace Simple.Messaging.RabbitMq
{
    public class RabbitMqMessageSubscriber<T>:IMessageSubscriber<T>
    {
        private volatile bool _isSubscribed;
        private string _uri;
        private readonly IExchangeNameFactory _exchangeNameFactory;
        private readonly IQueueNameFactory _queueNameFactory;
        private readonly IRoutingKeyFactory _routingKeyFactory;
        private readonly IMessageFormatter _messageFormatter;
        private readonly QueueConfiguration _configuration;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMqMessageSubscriber(string uri, IExchangeNameFactory exchangeNameFactory, IQueueNameFactory queueNameFactory, IRoutingKeyFactory routingKeyFactory, IMessageFormatter messageFormatter, QueueConfiguration configuration)
        {
            _uri = uri;
            _exchangeNameFactory = exchangeNameFactory;
            _queueNameFactory = queueNameFactory;
            _routingKeyFactory = routingKeyFactory;
            _messageFormatter = messageFormatter;
            _configuration = configuration;
        }

        public bool IsSubscribed { get { return _isSubscribed; } }

        public void Subscribe(Action<IMessageContext<T>> messageHandler)
        {
            _isSubscribed = true;

            var connectionFactory = new ConnectionFactory { Uri = _uri };

            _connection = connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            
            var queueName = ConfigureQueue(_channel);

            SubscribeToMessagesOnQueue(_channel,queueName,messageHandler);
            
        }

        private void SubscribeToMessagesOnQueue(IModel channel, string queueName, Action<IMessageContext<T>> messageHandler)
        {
            var consumer = new QueueingBasicConsumer(channel);
            channel.BasicConsume(queueName, false, consumer);

            Action taskAction = () => PollForMessages(channel, messageHandler, consumer);
            var task = new Task(taskAction);
            task.Start();
            
        }

        private void PollForMessages(IModel channel, Action<IMessageContext<T>> messageHandler, QueueingBasicConsumer consumer)
        {
            while (_isSubscribed)
            {
                try
                {
                    var message = (RabbitMQ.Client.Events.BasicDeliverEventArgs) consumer.Queue.Dequeue();

                    var context = new RabbitMqMessageContext<T>(channel, message.DeliveryTag);
                    context.Message = _messageFormatter.Read<T>(message.Body);

                    messageHandler(context);
                }
                catch (OperationInterruptedException)
                {
                }
            }
        }

        private string ConfigureQueue(IModel channel)
        {
            var exchange = _exchangeNameFactory.Build<T>();
            var routingKey = _routingKeyFactory.Build<T>();
            var queue = _queueNameFactory.Build<T>();

            var basicProperties = channel.CreateBasicProperties();
            basicProperties.ContentType = _messageFormatter.GetMIMEContentType();

            channel.QueueDeclare(queue, _configuration.IsQueueDurable, _configuration.IsQueueExclusive,_configuration.SupportsAutoDelete, null);
            channel.QueueBind(queue, exchange, routingKey);

            return queue;
        }

        public void Unsubscribe()
        {
            if(_channel != null)
                _channel.Close();

            if(_connection != null)
                _connection.Close();

            _isSubscribed = false;

        }

        public void Dispose()
        {
            if(_isSubscribed)
                Unsubscribe();
        }

    }
}