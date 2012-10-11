using Amazon.SQS;
using Amazon.SQS.Model;

namespace Simple.Messaging.AmazonSqs
{
    public class AmazonSqsMessageContext<T> : IMessageContext<T>
    {
        private readonly AmazonSQSClient _client;
        private readonly Message _message;
        private readonly string _uri;

        public AmazonSqsMessageContext(AmazonSQSClient client, Message message, string uri)
        {
            _client = client;
            _message = message;
            _uri = uri;
        }

        public void Accept()
        {
            var request = new DeleteMessageRequest()
                .WithQueueUrl(_uri)
                .WithReceiptHandle(_message.ReceiptHandle);

            _client.DeleteMessage(request);
        }

        public void Reject()
        {

        }

        public T Message { get; set; }
    }
}