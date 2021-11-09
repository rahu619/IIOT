using System.Collections.Generic;

namespace IIOT.MessageBroker.Configuration
{
    public class BrokerConfiguration
    {
        public IEnumerable<SimulatorClient> SimulatorClients { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public int ConnectionBacklog { get; set; }
        public string StoragePath { get; set; }
    }

    public class SimulatorClient
    {
        public string ClientId { get; set; }
        public string Topic { get; set; }

    }
}
