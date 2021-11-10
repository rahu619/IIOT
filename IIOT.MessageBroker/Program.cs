﻿using IIOT.MessageBroker.Configuration;
using IIOT.MessageBroker.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IIOT.MessageBroker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IConfiguration Configuration = new ConfigurationBuilder()
                                                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                                    .AddUserSecrets<Program>()
                                                    .Build();

            var brokerConfiguration = Configuration.GetRequiredSection(nameof(BrokerConfiguration));

            var serviceProvider = new ServiceCollection()
                    .AddLogging(builder => builder.AddConsole())
                    .Configure<BrokerConfiguration>(brokerConfiguration)
                    .AddSingleton<IBrokerService, BrokerService>()
                    .BuildServiceProvider();

            var loggerfactory = serviceProvider.GetService<ILoggerFactory>();
            var logger = loggerfactory.CreateLogger<Program>();
            logger.LogInformation("Starting IIOT Message Broker.");

            var service = serviceProvider.GetRequiredService<IBrokerService>();
            await service.Start();

            await Task.Run(() => Thread.Sleep(Timeout.Infinite));
            //Console.ReadKey();

            //await service.Stop();
            //logger.LogInformation("Stopping IIOT Message Broker.");

        }

    }
}
