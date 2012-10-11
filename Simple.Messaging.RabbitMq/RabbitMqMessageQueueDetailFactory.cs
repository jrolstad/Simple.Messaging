using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
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
            var queue = GetQueueDetails<T>(uri);
           
            if(queue == null)
            {
                return new MessageQueueDetail
                    {
                        Exists = false,
                        MessageCount = null,
                        Uri = _queueNameFactory.Build<T>()
                    };
            }

            return new MessageQueueDetail
                {
                    Exists = true,
                    MessageCount = Convert.ToInt32(queue.MessageCount),
                    Uri = queue.QueueName
                };
        }

        private QueueDeclareOk GetQueueDetails<T>(string server)
        {
            var connectionFactory = new ConnectionFactory { Uri = server };

            using (var connection = connectionFactory.CreateConnection())
            using (var model = connection.CreateModel())
            {
                try
                {
                    var queueName = _queueNameFactory.Build<T>();
                    var queue = model.QueueDeclarePassive(queueName);

                    return queue;
                }
                catch (OperationInterruptedException exception)
                {
                    if (IsQueueMissing(exception))
                        return null;
                    
                    throw;
                }
            }
        }

        private bool IsQueueMissing(OperationInterruptedException exception)
        {
            var reason = exception.ShutdownReason;

            const int missingQueueReplyCode = 404;
            return reason.ReplyCode == missingQueueReplyCode;
        }

        public void Dispose()
        {
            // Nothing to see here.  Move along.
        }

    }
}