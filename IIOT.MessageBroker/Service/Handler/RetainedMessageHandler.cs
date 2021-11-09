using MQTTnet;
using MQTTnet.Server;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace IIOT.MessageBroker.Service.Handler
{
    public class RetainedMessageHandler : IMqttServerStorage
    {
        private readonly string _fileName;

        public RetainedMessageHandler(string storagePath)
        {
            this._fileName = storagePath;
        }

        public Task SaveRetainedMessagesAsync(IList<MqttApplicationMessage> messages)
        {
            File.WriteAllText(this._fileName, JsonConvert.SerializeObject(messages));
            return Task.FromResult(0);
        }

        public Task<IList<MqttApplicationMessage>> LoadRetainedMessagesAsync()
        {
            IList<MqttApplicationMessage> retainedMessages;
            if (File.Exists(this._fileName))
            {
                var json = File.ReadAllText(this._fileName);
                retainedMessages = JsonConvert.DeserializeObject<List<MqttApplicationMessage>>(json);
            }
            else
            {
                retainedMessages = new List<MqttApplicationMessage>();
            }

            return Task.FromResult(retainedMessages);
        }
    }
}
