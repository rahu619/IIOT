using IIOT.MessageConsumer.Configuration;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using System.Text;

namespace IIOT.Service;

public class ConsumerService : BackgroundService
{
    private readonly ILogger<ConsumerService> _logger;
    private readonly ConsumerConfiguration _configuration;
    private readonly IManagedMqttClient _managedMqttClient;

    public ConsumerService(ILogger<ConsumerService> logger, IOptions<ConsumerConfiguration> configuration)
    {
        this._logger = logger;
        this._configuration = configuration?.Value ?? throw new ArgumentNullException(nameof(configuration));
        this._managedMqttClient = new MqttFactory().CreateManagedMqttClient();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            try
            {
                await SubscribeAsync();
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }

        await StopAsync();
    }

    public async Task SubscribeAsync()
    {
        this._managedMqttClient.UseApplicationMessageReceivedHandler(msg =>
        {
            var payloadText = Encoding.UTF8.GetString(msg?.ApplicationMessage?.Payload ?? Array.Empty<byte>());

            _logger.LogInformation($"Received msg: {payloadText}");

            // call service

        });

        await _managedMqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(_configuration.Topic).Build());
        await _managedMqttClient.StartAsync(GetClientOptions()).WaitAsync(CancellationToken.None);

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
               .WithTcpServer(_configuration.Server)
               //.WithTls()
               .Build()).Build();
    }
}
