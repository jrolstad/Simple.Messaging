using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace Simple.Messaging.AmazonSqs
{
    public class AmazonSqsMessageSubscriber<T>:IMessageSubscriber<T>
    {
        private readonly AmazonSQSClient _client;
        private readonly string _uri;
        private readonly IMessageFormatter _messageFormatter;
        private volatile bool _isSubscribed;

        public AmazonSqsMessageSubscriber(AmazonSQSClient client, string uri, IMessageFormatter messageFormatter)
        {
            _client = client;
            _uri = uri;
            _messageFormatter = messageFormatter;
        }

        public bool IsSubscribed { get { return _isSubscribed; } }

        public void Subscribe(Action<IMessageContext<T>> messageHandler)
        {
            _isSubscribed = true;

            Action taskAction = () => PollForMessages(messageHandler);
            var task = new Task(taskAction);
            task.Start();
        }

        private void PollForMessages(Action<IMessageContext<T>> messageHandler)
        {

                while (_isSubscribed)
                {
                    var message = ReadMessageFromQueue();
                    if(message!=null)
                    {
                        var body = _messageFormatter.Read<T>(message.Body);

                        var context = new AmazonSqsMessageContext<T>(_client,message,_uri) { Message = body };
                        messageHandler(context);
                    }
                   
                }
            
        }

       

        private Message ReadMessageFromQueue()
        {
            var request = new ReceiveMessageRequest()
                .WithQueueUrl(_uri)
                .WithMaxNumberOfMessages(1);

            var response = _client.ReceiveMessage(request);
            var message = response.ReceiveMessageResult.Message.FirstOrDefault();
            
            return message;
        }

        public void Unsubscribe()
        {
            _isSubscribed = false;
        }

        public void Dispose()
        {
            if (_isSubscribed)
                Unsubscribe();
        }
    }
}