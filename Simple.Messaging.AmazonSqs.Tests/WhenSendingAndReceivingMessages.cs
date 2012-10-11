using System;
using System.Collections.Generic;
using System.Threading;
using Amazon.SQS;
using Amazon.SQS.Model;
using NUnit.Framework;
using Simple.Messaging.AmazonSqs.Tests.Messages;

namespace Simple.Messaging.AmazonSqs.Tests
{
    [TestFixture]
    [Category("Integration")]
    public class WhenSendingAndReceivingMessages
    {
        [Test]
        public void When_sending_a_message_it_can_be_received()
        {
            // Arrange
            var message = new TestMessage { Identifier = Guid.NewGuid().ToString(), Description = "something", SentAt = DateTime.Now };

            var receivedMessages = new List<TestMessage>();

            var client = new AmazonSQSClient("access key", "secret key");
            var response = client.CreateQueue(new CreateQueueRequest().WithQueueName("testQueue"));
            var uri = response.CreateQueueResult.QueueUrl;

            var receiver = new AmazonSqsMessageSubscriber<TestMessage>(client, uri, new JsonMessageFormatter());
            receiver.Subscribe(s =>
            {
                receivedMessages.Add(s.Message);
                s.Accept();
            });

            var sender = new AmazonSqsMessenger(client, uri, new JsonMessageFormatter());

            // Act
            sender.Send(message);

            Thread.Sleep(5000);

            // Assert
            Assert.That(receivedMessages, Is.Not.Empty);
        }
    }
}