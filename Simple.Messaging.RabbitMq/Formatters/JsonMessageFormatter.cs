using Newtonsoft.Json;

namespace Simple.Messaging.RabbitMq.Formatters
{
    public class JsonMessageFormatter:IMessageFormatter
    {
        public byte[] Write<T>(T message)
        {
            var json = JsonConvert.SerializeObject(message);
            var body = System.Text.Encoding.UTF8.GetBytes(json);

            return body;
        }

        public T Read<T>(byte[] message)
        {
            var json = System.Text.Encoding.UTF8.GetString(message);

            return JsonConvert.DeserializeObject<T>(json);
        }

        public string GetMIMEContentType()
        {
            return "text/json";
        }
    }
}