namespace IIOT.MessageConsumer.Configuration
{
    public class ConsumerConfiguration
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string Topic { get; set; }
        public string ClientId { get; set; }
        public int AutoReconnectDelay { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
