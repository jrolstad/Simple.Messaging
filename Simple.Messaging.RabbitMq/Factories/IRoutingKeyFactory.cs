namespace Simple.Messaging.RabbitMq.Factories
{
    public interface IRoutingKeyFactory
    {
        string Build<T>(); 
    }
}