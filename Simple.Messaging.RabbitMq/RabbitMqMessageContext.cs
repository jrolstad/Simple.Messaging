using RabbitMQ.Client;

namespace Simple.Messaging.RabbitMq
{
    public class RabbitMqMessageContext<T>:IMessageContext<T>
    {
        private readonly IModel _channel;
        private readonly ulong _deliveryTag;

        public RabbitMqMessageContext(IModel channel,ulong deliveryTag)
        {
            _channel = channel;
            _deliveryTag = deliveryTag;
        }

        public void Accept()
        {
            _channel.BasicAck(_deliveryTag, false);
        }

        public void Reject()
        {
            _channel.BasicReject(_deliveryTag,false);
        }

        public T Message { get; set; }
    }
}