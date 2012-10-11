using Newtonsoft.Json;

namespace Simple.Messaging.AmazonSqs
{
    public class JsonMessageFormatter:IMessageFormatter
    {
        public string Write<T>(T message)
        {
            return JsonConvert.SerializeObject(message);
        }

        public T Read<T>(string body)
        {
            return JsonConvert.DeserializeObject<T>(body);
        }
    }
}