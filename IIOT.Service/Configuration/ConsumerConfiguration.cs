namespace IIOT.MessageConsumer.Configuration
{
    public class ConsumerConfiguration
    {
        public string Server { get; set; }
        public string Topic { get; set; }
        public string ClientId { get; set; }
        public int AutoReconnectDelay { get; set; }
    }
}
