namespace Simple.Messaging.AmazonSqs
{
    public interface IMessageFormatter
    {
        string Write<T>(T message);

        T Read<T>(string body);
    }
}
