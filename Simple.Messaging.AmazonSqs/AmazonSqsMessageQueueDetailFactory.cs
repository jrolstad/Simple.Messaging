using System.Linq;
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
            var listResponse = _client.ListQueues(new ListQueuesRequest().WithQueueNamePrefix(uri));
            var exists = listResponse.ListQueuesResult.QueueUrl.Any();

            if (exists)
            {
                var queue = _client.GetQueueUrl(new GetQueueUrlRequest().WithQueueName(uri));
                var request = new GetQueueAttributesRequest().WithQueueUrl(queue.GetQueueUrlResult.QueueUrl);
                var response = _client.GetQueueAttributes(request);

                return new MessageQueueDetail
                    {
                        MessageCount = response.GetQueueAttributesResult.ApproximateNumberOfMessages,
                        Uri = queue.GetQueueUrlResult.QueueUrl,
                        Exists = true
                    };
            }
            else
            {
                return new MessageQueueDetail
                    {
                         MessageCount = null,
                         Uri = uri,
                         Exists = false
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