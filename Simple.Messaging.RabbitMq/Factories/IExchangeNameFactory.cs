namespace Simple.Messaging.RabbitMq.Factories
{
    public interface IExchangeNameFactory
    {
        string Build<T>();
    }
}