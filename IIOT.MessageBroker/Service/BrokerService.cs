using IIOT.MessageBroker.Configuration;
using IIOT.MessageBroker.Service.Handler;
using IIOT.MessageConsumer.Data.Entities;
using IIOT.MessageConsumer.Data.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Protocol;
using MQTTnet.Server;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace IIOT.MessageBroker.Service
{
    public class BrokerService : IBrokerService
    {
        private readonly IMqttServer _mqttServer;
        private readonly ILogger _logger;
        private readonly BrokerConfiguration _configuration;
        public BrokerService(ILogger<BrokerService> logger, IOptions<BrokerConfiguration> configuration)
        {
            //creating mqtt server
            this._mqttServer = new MqttFactory().CreateMqttServer();
            this._logger = logger;
            this._configuration = configuration?.Value ?? throw new ArgumentNullException(nameof(configuration));
        }


        // Create the options for our MQTT Broker
        private MqttServerOptionsBuilder GetOptionsBuilder()
        {
            return new MqttServerOptionsBuilder()
                                                 .WithConnectionValidator(c =>
                                                 {
                                                     this._logger.LogInformation("{ClientId} connection validator for c.Endpoint : {Endpoint}", c.ClientId, c.Endpoint);

                                                     if (_configuration.Username != c.Username || _configuration.Password != c.Password)
                                                     {
                                                         c.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                                                         return;
                                                     }

                                                     c.ReasonCode = MqttConnectReasonCode.Success;

                                                 })
                                                 .WithApplicationMessageInterceptor(context =>
                                                 {
                                                     this._logger.LogDebug("Intercepting messages.");

                                                     var client = _configuration.SimulatorClients.SingleOrDefault(x => x.ClientId == context.ClientId);
                                                     if (client is null)
                                                     {
                                                         _logger.LogError("Invalid client found : {ClientId}", context.ClientId);
                                                         context.AcceptPublish = false;
                                                         return;
                                                     }

                                                     if (context.ApplicationMessage.Topic == client.Topic)
                                                     {
                                                         var temperature = new Temperature
                                                         {
                                                             ClientId = context.ClientId,
                                                             ReceivedTime = DateTime.Now,
                                                             Value = Encoding.UTF8.GetString(context.ApplicationMessage.Payload)
                                                         };

                                                         context.ApplicationMessage.Payload = temperature.Serialize();

                                                         _logger.LogInformation(Encoding.UTF8.GetString(context.ApplicationMessage.Payload));
                                                     }

                                                 })
                                                 .WithConnectionBacklog(_configuration.ConnectionBacklog)
                                                 .WithDefaultEndpointPort(_configuration.Port)
                                                 .WithPersistentSessions()
                                                 .WithStorage(new RetainedMessageHandler(_configuration.StoragePath));
            ;
        }

        public async Task Start()
        {
            // start the server with options  
            this._logger.LogInformation("Broker is running");
            await _mqttServer.StartAsync(GetOptionsBuilder().Build());

        }

        public async Task Stop()
        {
            await _mqttServer.StopAsync();
        }


    }
}
