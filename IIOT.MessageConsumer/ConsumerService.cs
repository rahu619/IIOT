using IIOT.MessageConsumer.Configuration;
using IIOT.MessageConsumer.Data.Entities;
using IIOT.MessageConsumer.Data.Extensions;
using IIOT.MessageConsumer.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IIOT.MessageConsumer
{
    public class ConsumerService : BackgroundService
    {
        private readonly ILogger<ConsumerService> _logger;
        private readonly IManagedMqttClient _managedMqttClient;
        private readonly ITemperatureService _temperatureService;
        private readonly ConsumerConfiguration _configuration;

        public ConsumerService(ILogger<ConsumerService> logger, IOptions<ConsumerConfiguration> configuration, IServiceProvider serviceProvider)
        {
            this._logger = logger;
            this._configuration = configuration?.Value ?? throw new ArgumentNullException(nameof(configuration));
            this._temperatureService = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ITemperatureService>();
            this._managedMqttClient = new MqttFactory().CreateManagedMqttClient();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                try
                {
                    await SubscribeAsync(stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
            }

            await StopAsync();
        }

        public async Task SubscribeAsync(CancellationToken stoppingToken)
        {
            this._managedMqttClient.UseApplicationMessageReceivedHandler(msg =>
            {
                var content = msg?.ApplicationMessage?.Payload;
                if (content != null)
                {
                    var temperatureObj = content.Deserialize<Temperature>();
                    _logger.LogInformation($"Received msg: {temperatureObj.Value}");

                    try
                    {
                        this._temperatureService.AddTemperature(temperatureObj);
                    }
                    catch (Exception e)
                    {
                        _logger.LogDebug(e.ToString());
                        _logger.LogInformation("Error entering temperature:{Value} of Client:{ClientId}", temperatureObj.Value, temperatureObj.ClientId);
                    }
                }

            });

            await _managedMqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(_configuration.Topic).Build());
            await _managedMqttClient.StartAsync(GetClientOptions());

            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);

        }

        public async Task StopAsync()
        {
            await _managedMqttClient.StopAsync();
        }

        /// <summary>
        /// Using managed MQTT client : considering features like auto-reconnecting,  internal queue, no manual subscription after connection is lost.
        /// </summary>
        /// <returns></returns>
        private ManagedMqttClientOptions GetClientOptions()
        {
            return new ManagedMqttClientOptionsBuilder()
                   .WithAutoReconnectDelay(TimeSpan.FromSeconds(_configuration.AutoReconnectDelay))
                   .WithClientOptions(new MqttClientOptionsBuilder()
                   .WithClientId(_configuration.ClientId)
                   .WithCredentials(_configuration.Username, _configuration.Password)
                   .WithTcpServer(_configuration.Server, _configuration.Port)
                   .WithCleanSession(false)
                   //.WithTls()
                   .Build()).Build();
        }
    }

}
