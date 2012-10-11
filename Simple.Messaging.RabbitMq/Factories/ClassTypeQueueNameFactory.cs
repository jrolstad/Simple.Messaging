namespace Simple.Messaging.RabbitMq.Factories
{
    public class ClassTypeQueueNameFactory:IQueueNameFactory
    {
        public string Build<T>()
        {
            var type = typeof(T);
            var name = type.FullName;

            return name;
        }
    }
}