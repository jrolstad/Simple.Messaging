using System.Messaging;

namespace Simple.Messaging.MsMq
{
    public class MsMqMessageQueueDetailFactory:IMessageQueueDetailFactory
    {
        public MessageQueueDetail Build<T>(string uri)
        {
            var queue = new MessageQueue(uri, QueueAccessMode.SendAndReceive);

            var exists = MessageQueue.Exists(uri);

            if (exists)
            {
                var messageCount = queue.GetAllMessages().Length;
                var detail = new MessageQueueDetail
                    {
                        MessageCount = messageCount,
                        Uri = queue.Path,
                        Exists = true
                    };

                return detail;
            }
            else
            {
                return new MessageQueueDetail
                    {
                        MessageCount = null,
                        Uri = queue.Path,
                        Exists = false
                    };
            }
        }

        public void Dispose()
        {
            // Nothing to see here.  Move along.
        }
    }
}