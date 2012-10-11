namespace Simple.Messaging.RabbitMq.Factories
{
    public class EmptyRoutingKeyFactory:IRoutingKeyFactory
    {
        public string Build<T>()
        {
            return "";
        }
    }
}