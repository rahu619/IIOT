using IIOT.MessageConsumer.Data.Entities;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace IIOT.MessageConsumer.Service.Hubs
{
    public class TemperatureHub : Hub
    {
        public async Task SendTemperature(Temperature temperature)
        {
            await Clients.All.SendAsync("ReceiveMessage", temperature);
        }
    }
}
