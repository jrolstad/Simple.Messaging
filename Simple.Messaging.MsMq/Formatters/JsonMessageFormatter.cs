using System;
using System.IO;
using System.Messaging;
using System.Text;
using Newtonsoft.Json;

namespace Messaging.MsMq.Formatters
{
    public class JsonMessageFormatter:IMessageFormatter
    {
        private readonly Type _messageType;

        public JsonMessageFormatter(Type messageType)
        {
            _messageType = messageType;
        }

        public object Clone()
        {
            return this;
        }

        public bool CanRead(Message message)
        {
            return true;
        }

        public object Read(Message message)
        {
            //Obtain the BodyStream for the message.
            var stm = message.BodyStream;

            //Create a StreamReader object used for reading from the stream.
            var reader = new StreamReader(stm);

            //Return the string read from the stream.
            //StreamReader.ReadToEnd returns a string.
            var body = reader.ReadToEnd();

            var typedMessage = JsonConvert.DeserializeObject(body,_messageType);
            

            return typedMessage;
        }

        public void Write(Message message, object obj)
        {
            var body = JsonConvert.SerializeObject(obj);

            //Declare a buffer.
            byte[] buff;

            //Place the string into the buffer using UTF8 encoding.
            buff = Encoding.UTF8.GetBytes(body);

            //Create a new MemoryStream object passing the buffer.
            Stream stm = new MemoryStream(buff);

            //Assign the stream to the message's BodyStream property.
            message.BodyStream = stm;
            
        }
    }
}