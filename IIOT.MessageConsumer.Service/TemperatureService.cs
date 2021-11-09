using IIOT.MessageConsumer.Data.Entities;
using IIOT.MessageConsumer.Data.Repositories;
using IIOT.MessageConsumer.Service.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IIOT.MessageConsumer.Service
{
    public class TemperatureService : ITemperatureService
    {
        private readonly ITemperatureRepository _temperatureRepository;
        private readonly IHubContext<TemperatureHub> _hubContext;

        public TemperatureService(ITemperatureRepository temperatureRepository, IHubContext<TemperatureHub> hubContext)
        {
            this._temperatureRepository = temperatureRepository;
            this._hubContext = hubContext;
        }

        public async Task<IEnumerable<Temperature>> GetAllTemperatures() =>
             await this._temperatureRepository.GetAllTemperaturesAsync();

        public async Task AddTemperature(Temperature temperature)
        {
            await this._temperatureRepository.AddTemperatureAsync(temperature);
            await this._hubContext.Clients.All.SendAsync("ReceiveMessage", temperature);
        }

    }
}
