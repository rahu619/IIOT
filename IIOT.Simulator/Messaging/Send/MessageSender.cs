﻿using IIOT.Simulator.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;

namespace IIOT.Simulator.Messaging.Send
{
    public class MessageSender
    {
        private readonly ILogger _logger;
        private readonly IMqttClient _mqttClient;

        private readonly Timer _failureTimer;
        private readonly MessageSenderConfiguration _configuration;
        public MessageSender(ILogger<MessageSender> logger, IOptions<MessageSenderConfiguration> configuration, ISensor sensor)
        {
            _logger = logger;
            sensor.MessageReceived += SensorMessageReceived;

            _configuration = configuration?.Value ?? throw new ArgumentNullException(nameof(configuration));
            _failureTimer = new Timer(SensorFailureEvent, null, _configuration.MessageFailureTimeInterval, Timeout.Infinite);
            _mqttClient = new MqttFactory().CreateMqttClient();
            _mqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(OnConnected);
            _mqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(OnDisconnected);


        }

        private void SensorFailureEvent(object state)
        {
            _logger.LogError("Error retrieving value from sensor!");
        }


        public async Task SensorMessageReceived(string value)
        {
            _failureTimer.Change(_configuration.MessageFailureTimeInterval, Timeout.Infinite);

            await ConnectAsync();

            try
            {
                var message = new MqttApplicationMessageBuilder()
                                .WithTopic(_configuration.Topic)
                                .WithPayload(value)
                                .WithExactlyOnceQoS()
                                .WithRetainFlag()
                                .Build();

                await _mqttClient.PublishAsync(message, CancellationToken.None);

            }
            catch (Exception ex)
            {
                _logger.LogError("Error in Fn[SensorMessageReceived]: {exception}", ex.Message);
                _logger.LogDebug(ex.ToString());
            }

        }

        private async Task ConnectAsync()
        {
            if (!_mqttClient.IsConnected)
            {
                try
                {
                    await _mqttClient.ConnectAsync(ClientOptions).WaitAsync(CancellationToken.None);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error connecting to message broker!");
                }
            }
        }

        private IMqttClientOptions ClientOptions => new MqttClientOptionsBuilder()
                                                    .WithClientId(_configuration.Client)
                                                    .WithTcpServer(_configuration.Server, _configuration.Port)
                                                    .WithCredentials(_configuration.Username, _configuration.Password)
                                                    .WithCleanSession(false)
                                                    //.WithTls()
                                                    .Build();

        //new MqttClientOptions
        //{
        //    ClientId = Guid.NewGuid().ToString("D"),
        //    ProtocolVersion = MqttProtocolVersion.V500,
        //    ChannelOptions = new MqttClientTcpOptions
        //    {
        //        Server = _configuration.Server,
        //        Port = _configuration.Port
        //    },
        //    Credentials = new MqttClientCredentials
        //    {
        //        //TODO: dotnet secret manager
        //        Username = _configuration.Username,
        //        Password = Encoding.UTF8.GetBytes(_configuration.Password)
        //    },
        //    CleanSession = true,
        //    KeepAlivePeriod = TimeSpan.FromSeconds(60)
        //};




        #region Events
        public void OnConnected(MqttClientConnectedEventArgs obj)
        {
            _logger.LogInformation("Successfully connected [{RunTime}]", DateTime.Now);
        }

        public void OnDisconnected(MqttClientDisconnectedEventArgs obj)
        {
            _logger.LogInformation("Disconnected successfully [{RunTime}]", DateTime.Now);
        }

        #endregion
    }
}
