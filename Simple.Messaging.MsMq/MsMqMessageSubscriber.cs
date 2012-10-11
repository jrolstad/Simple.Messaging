using System;
using System.Messaging;
using System.Threading.Tasks;

namespace Simple.Messaging.MsMq
{
    public class MsMqMessageSubscriber<T>:IMessageSubscriber<T>
    {
        private readonly string _uri;
        private readonly IMessageFormatter _messageFormatter;
        private volatile bool _isSubscribed ;

        public MsMqMessageSubscriber(string uri, IMessageFormatter messageFormatter)
        {
            _uri = uri;
            _messageFormatter = messageFormatter;
        }

        public void Subscribe(Action<IMessageContext<T>> messageHandler)
        {
            _isSubscribed = true;

            Action taskAction = () => PollForMessages(messageHandler);
            var task = new Task(taskAction);
            task.Start();
        }

        public bool IsSubscribed { get { return _isSubscribed; } }

        private void PollForMessages(Action<IMessageContext<T>> messageHandler)
        {
            using (var messageQueue = new MessageQueue(_uri, QueueAccessMode.Receive) {Formatter = _messageFormatter})
            {
                while (_isSubscribed)
                {
                    try
                    {
                        var body = ReadMessageFromQueue(messageQueue);
                        HandleMessage(messageHandler, body);
                    }
                    catch (MessageQueueException exception)
                    {
                        // Determine if the exception was due to no message being on the queue
                        if (exception.MessageQueueErrorCode != MessageQueueErrorCode.IOTimeout)
                        {
                            throw;
                        }
                    }
                }
            }
        }

        private static void HandleMessage(Action<IMessageContext<T>> messageHandler, T body)
        {
            var context = new MsMqMessageContext<T> {Message = body};
            messageHandler(context);
        }

        private T ReadMessageFromQueue(MessageQueue messageQueue)
        {
            var defaultTimeOut = new TimeSpan(0, 0, 0, 1); // Look for a message for 1 second; if none, then timeout
            var message = messageQueue.Receive(defaultTimeOut);
            
            var body = (T) message.Body;
            return body;
        }

        public void Unsubscribe()
        {
            _isSubscribed = false;
        }

        public void Dispose()
        {

            if(_isSubscribed)
                Unsubscribe();
        }

    }
}