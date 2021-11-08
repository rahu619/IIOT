using IIOT.Simulator.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IIOT.Simulator.Messaging
{
    internal class Sensor : ISensor
    {
        private const string UNIT_OF_TEMPERATURE = "°C";
        private readonly ILogger _logger;
        private readonly SensorConfiguration _configuration; 
        public event ISensor.MessageReceivedHandler MessageReceived; 

        public Sensor(ILogger<Sensor> logger, IOptions<SensorConfiguration> options)
        {
            this._logger = logger;
            this._configuration = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

         
        public async Task Generate()
        {
            while (true)
            {
                var temperature = GetRandomTemperature();

                //received message
                MessageReceived(temperature);

                _logger.LogInformation("Temperature : {Temperature} [{RunTime}]", temperature, DateTime.Now);
                await Task.Delay(_configuration.TimeInterval);
            }

        }

        /// <summary>
        /// Returns a random temperature value between the min-max range
        /// </summary>
        /// <returns></returns>
        private string GetRandomTemperature()
        {
            Random random = new();
            int value = -1;
            try
            {
                value = random.Next(_configuration.MinTemperature, _configuration.MaxTemperature);
            }
            catch (ArgumentOutOfRangeException)
            {
                this._logger.LogError("Temperature value is out of range!");
            }

            string randomTemperature = $"{value}{UNIT_OF_TEMPERATURE}";
            this._logger.LogDebug(randomTemperature);

            return randomTemperature;
        }





    }
}
