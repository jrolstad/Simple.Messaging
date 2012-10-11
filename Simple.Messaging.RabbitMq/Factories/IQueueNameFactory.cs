namespace Simple.Messaging.RabbitMq.Factories
{
    public interface IQueueNameFactory
    {
        string Build<T>(); 
    }
}