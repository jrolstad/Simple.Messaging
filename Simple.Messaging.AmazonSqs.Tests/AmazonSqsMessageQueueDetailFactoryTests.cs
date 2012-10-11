using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Amazon.SQS;
using Amazon.SQS.Model;
using NUnit.Framework;
using Simple.Messaging.AmazonSqs.Tests.Messages;

namespace Simple.Messaging.AmazonSqs.Tests
{
    [TestFixture]
    public class AmazonSqsMessageQueueDetailFactoryTests
    {

        [Test]
        [TestCase(@"testQueue", true)]
        [TestCase(@"doesNotExist", false)]
        public void When_obtaining_details_on_a_message_queue(string queueName, bool exists)
        {
            // Arrange
            var client = new AmazonSQSClient("access key", "secret key");

            if (exists)
            {
                client.CreateQueue(new CreateQueueRequest().WithQueueName(queueName));
            }
            else
            {
                var response = client.ListQueues(new ListQueuesRequest().WithQueueNamePrefix(queueName));
                if (response.ListQueuesResult.QueueUrl.Any())
                {
                    client.DeleteQueue(new DeleteQueueRequest().WithQueueUrl(response.ListQueuesResult.QueueUrl.First()));
                }
            }

            Thread.Sleep(2000); // Give the queues some time to spin up
          
            var factory = new AmazonSqsMessageQueueDetailFactory(client);


            // Act
            var result = factory.Build<TestMessage>(queueName);

            // Assert
            Assert.That(result.Uri, Is.StringContaining(queueName));
            Assert.That(result.Exists, Is.EqualTo(exists));
        }  

    }
}