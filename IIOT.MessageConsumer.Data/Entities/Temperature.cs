using System;

namespace IIOT.MessageConsumer.Data.Entities
{
    public class Temperature
    {
        public Guid Id { get; set; }
        public string ClientId { get; set; }
        public string Value { get; set; }
        public DateTime ReceivedTime { get; set; }

    }
}
