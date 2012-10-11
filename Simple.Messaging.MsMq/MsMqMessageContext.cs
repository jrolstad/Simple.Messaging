namespace Simple.Messaging.MsMq
{
    public class MsMqMessageContext<T>:IMessageContext<T>
    {
        public void Accept()
        {
            // Not implemented
        }

        public void Reject()
        {
            // Not implemented
        }

        public T Message { get; set; }
    }
}