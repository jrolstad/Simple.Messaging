using System.Messaging;

namespace Simple.Messaging.MsMq
{
    public class MsMqMessenger:IMessenger
    {
        private readonly string _uri;
        private readonly IMessageFormatter _messageFormatter;

        public MsMqMessenger(string uri, IMessageFormatter messageFormatter)
        {
            _uri = uri;
            _messageFormatter = messageFormatter;
        }

        public void Dispose()
        {
            // Nothing to see here. Move along.
        }

        public void Send<T>(T message)
        {
            using (var messageQueue = new MessageQueue(_uri, QueueAccessMode.Send) { Formatter = _messageFormatter })
            {
                messageQueue.Send(message);
            }
        }
    }
}