namespace Simple.Messaging.RabbitMq.Formatters
{
    public interface IMessageFormatter
    {
        byte[] Write<T>(T message);

        T Read<T>(byte[] message);

        string GetMIMEContentType();
    }
}