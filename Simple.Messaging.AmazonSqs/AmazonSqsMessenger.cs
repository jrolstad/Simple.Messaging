using Amazon.SQS;
using Amazon.SQS.Model;

namespace Simple.Messaging.AmazonSqs
{
    public class AmazonSqsMessenger:IMessenger
    {
        private readonly AmazonSQSClient _client;
        private readonly string _uri;
        private readonly IMessageFormatter _messageFormatter;

        public AmazonSqsMessenger(AmazonSQSClient client, string uri, IMessageFormatter messageFormatter)
        {
            _client = client;
            _uri = uri;
            _messageFormatter = messageFormatter;
        }

        public void Dispose()
        {
            // Nothing to see here.  Move along.
        }

        public void Send<T>(T message)
        {
            var formattedMessage = _messageFormatter.Write(message);

            var messageRequest = new SendMessageRequest()
                .WithQueueUrl(_uri)
                .WithMessageBody(formattedMessage);

            _client.SendMessage(messageRequest);
        }
    }
}