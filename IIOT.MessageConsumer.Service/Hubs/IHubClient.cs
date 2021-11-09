using IIOT.MessageConsumer.Data.Entities;
using System.Threading.Tasks;

namespace IIOT.MessageConsumer.Service.Hubs
{
    public interface IHubClient
    {
        Task SendTemperature(Temperature temperature);
    }
}
