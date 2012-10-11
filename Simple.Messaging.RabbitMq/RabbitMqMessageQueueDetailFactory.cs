using System;
using RabbitMQ.Client;
using Simple.Messaging.RabbitMq.Factories;

namespace Simple.Messaging.RabbitMq
{
    public class RabbitMqMessageQueueDetailFactory:IMessageQueueDetailFactory
    {
        private readonly IQueueNameFactory _queueNameFactory;

        public RabbitMqMessageQueueDetailFactory(IQueueNameFactory queueNameFactory)
        {
            _queueNameFactory = queueNameFactory;
        }

        public MessageQueueDetail Build<T>(string uri)
        {
            var connectionFactory = new ConnectionFactory {Uri = uri};



            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var queueNameFactory = _queueNameFactory.Build<T>();
                var queue = channel.QueueDeclarePassive(queueNameFactory);

                var detail = new MessageQueueDetail
                    {
                        MessageCount = Convert.ToInt32(queue.MessageCount),
                        Uri = queue.QueueName,
                        Exists = true
                    };

                channel.Close();
                connection.Close();

                return detail;



            }
        }

        public void Dispose()
        {
            // Nothing to see here.  Move along.
        }

    }
}