namespace IIOT.Simulator.Configuration
{
    public class SimulatorConfiguration
    {
        public SensorConfiguration SensorConfiguration { get; set; }
        public MessageSenderConfiguration MessageSenderConfiguration { get; set; }
    }
    public class SensorConfiguration
    {

        public int MinTemperature { get; set; }
        public int MaxTemperature { get; set; }

        /// <summary>
        /// Time interval in milli-seconds
        /// </summary>
        public int TimeInterval { get; set; }

    }


    public class MessageSenderConfiguration
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string Topic { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Client { get; set; }
        public int KeepAlivePeriod { get; set; }
        public int MessageFailureTimeInterval { get; set; }
    }
}
