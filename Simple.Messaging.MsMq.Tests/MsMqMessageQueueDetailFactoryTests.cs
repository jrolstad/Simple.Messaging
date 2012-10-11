using System.Messaging;
using System.Threading;
using NUnit.Framework;
using Simple.Messaging.MsMq.Tests.Messages;

namespace Simple.Messaging.MsMq.Tests
{
    [TestFixture]
    public class MsMqMessageQueueDetailFactoryTests
    {

        [Test]
        [TestCase(@".\private$\testQueue",true)]
        [TestCase(@".\private$\doesNotExist",false)]
        public void When_obtaining_details_on_a_message_queue(string queueName, bool exists)
        {
            // Arrange
            if (exists && !MessageQueue.Exists(queueName))
                MessageQueue.Create(queueName);
            else if (!exists && MessageQueue.Exists(queueName))
                MessageQueue.Delete(queueName);

            Thread.Sleep(2000); // Give the queues some time to spin up

            var factory = new MsMqMessageQueueDetailFactory();

            // Act
            var result = factory.Build<TestMessage>(queueName);

            // Assert
            Assert.That(result.Uri,Is.EqualTo(queueName));
            Assert.That(result.Exists,Is.EqualTo(exists));
        } 

    }
}