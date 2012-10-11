using System;

namespace Simple.Messaging
{
    public interface IMessenger : IDisposable
    {
        void Send<T>(T message);
    }
}