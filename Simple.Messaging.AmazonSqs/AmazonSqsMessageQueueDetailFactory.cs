using Amazon.SQS;
using Amazon.SQS.Model;

namespace Simple.Messaging.AmazonSqs
{
    public class AmazonSqsMessageQueueDetailFactory : IMessageQueueDetailFactory
    {
        private readonly AmazonSQSClient _client;

        public AmazonSqsMessageQueueDetailFactory(AmazonSQSClient client)
        {
            _client = client;
        }

        public MessageQueueDetail Build<T>(string uri)
        {
            var request = new GetQueueAttributesRequest().WithQueueUrl(uri);

            var response = _client.GetQueueAttributes(request);

            if (response.IsSetGetQueueAttributesResult())
            {
                return new MessageQueueDetail
                    {
                        MessageCount = response.GetQueueAttributesResult.ApproximateNumberOfMessages,
                        Uri = uri,
                        Exists = true
                    };
            }

            return new MessageQueueDetail {Exists = false, Uri = uri};
        }

        public void Dispose()
        {
            // Nothing to see here.  Move along.
        }
    }
}