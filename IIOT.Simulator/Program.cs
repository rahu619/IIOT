using IIOT.Simulator.Configuration;
using IIOT.Simulator.Messaging;
using IIOT.Simulator.Messaging.Send;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        IConfiguration Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .Build();

        var simulatorSection = Configuration.GetSection(nameof(SimulatorConfiguration));
        var sensorSection = simulatorSection.GetSection(nameof(SensorConfiguration));
        var senderSection = simulatorSection.GetSection(nameof(MessageSenderConfiguration));

        var serviceProvider = new ServiceCollection()
            .AddLogging(builder => builder.AddConsole())
            .Configure<SensorConfiguration>(sensorSection)
            .Configure<MessageSenderConfiguration>(senderSection)
            .AddSingleton<ISensor, Sensor>()
            .AddSingleton<MessageSender>()
            .BuildServiceProvider();


        var loggerfactory = serviceProvider.GetService<ILoggerFactory>();
        var logger = loggerfactory.CreateLogger<Program>();
        logger.LogInformation("Starting simulation environment");

        var sensor = serviceProvider.GetRequiredService<ISensor>();
        var messageSender = serviceProvider.GetRequiredService<MessageSender>();

        await sensor.Generate();

        logger.LogInformation("Ending simulation");

    }
}