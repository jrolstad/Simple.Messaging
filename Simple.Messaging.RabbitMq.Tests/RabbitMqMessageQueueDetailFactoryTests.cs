using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using Simple.Messaging.RabbitMq.Factories;
using Simple.Messaging.RabbitMq.Tests.Messages;

namespace Simple.Messaging.RabbitMq.Tests
{
    [TestFixture]
    public class RabbitMqMessageQueueDetailFactoryTests
    {
        [Test]
        public void When_obtaining_details_on_a_message_queue_and_it_exists()
        {
            // Arrange
            Thread.Sleep(2000); // Give the queues some time to spin up
            const string uri = @"amqp://devq:devq@sea-2600-53:5672/Dev_JoshR";

            var factory = new RabbitMqMessageQueueDetailFactory(new ClassTypeQueueNameFactory());

            // Act
            var result = factory.Build<TestMessage>(uri);

            // Assert
            Assert.That(result.Uri, Is.StringContaining("TestMessage"));
            Assert.That(result.Exists, Is.True);
        }

        [Test]
        public void When_obtaining_details_on_a_message_queue_and_it_does_not_exist()
        {
            // Arrange
            Thread.Sleep(2000); // Give the queues some time to spin up
            const string uri = @"amqp://devq:devq@sea-2600-53:5672/Dev_JoshR";

            var factory = new RabbitMqMessageQueueDetailFactory(new ClassTypeQueueNameFactory());

            // Act
            var result = factory.Build<string>(uri);

            // Assert
            Assert.That(result.Exists, Is.False);
        } 
         

    }
}