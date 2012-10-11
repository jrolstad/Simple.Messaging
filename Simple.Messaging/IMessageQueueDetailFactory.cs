using System;

namespace Simple.Messaging
{
    public interface IMessageQueueDetailFactory: IDisposable
    {
        MessageQueueDetail Build<T>(string uri);
    }
}