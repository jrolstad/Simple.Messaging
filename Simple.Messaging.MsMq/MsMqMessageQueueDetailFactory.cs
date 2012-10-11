using System.Messaging;

namespace Simple.Messaging.MsMq
{
    public class MsMqMessageQueueDetailFactory:IMessageQueueDetailFactory
    {
        public MessageQueueDetail Build<T>(string uri)
        {
            var queue = new MessageQueue(uri);

            var detail = new MessageQueueDetail
                {
                    MessageCount = queue.GetAllMessages().Length, 
                    Uri = queue.Path, 
                    Exists = MessageQueue.Exists(uri)
                };

            return detail;
        }

        public void Dispose()
        {
            // Nothing to see here.  Move along.
        }
    }
}