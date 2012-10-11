using System;
using System.Collections.Generic;
using System.Messaging;
using System.Threading;
using Messaging.MsMq.Formatters;
using NUnit.Framework;
using Simple.Messaging.MsMq.Tests.Messages;

namespace Simple.Messaging.MsMq.Tests
{
    [TestFixture]
    [Category("Integration")]
    public class WhenSendingAndReceivingMessages
    {
        [Test]
        public void When_sending_a_message_then_it_can_be_received()
        {
            // Arrange
            const string uri = @".\private$\testQueue";
            MessageQueue.Create(uri);

            var message = new TestMessage { Identifier = Guid.NewGuid().ToString(), Description = "something", SentAt = DateTime.Now };

            var receivedMessages = new List<TestMessage>();

            var receiver = new MsMqMessageSubscriber<TestMessage>(uri, new JsonMessageFormatter(typeof(TestMessage)));
            receiver.Subscribe(s => receivedMessages.Add(s.Message));

            var sender = new MsMqMessenger(uri, new JsonMessageFormatter(typeof(TestMessage)));

            // Act
            sender.Send(message);

            Thread.Sleep(5000);

            // Assert
            Assert.That(receivedMessages, Is.Not.Empty);
        }
         

    }
}