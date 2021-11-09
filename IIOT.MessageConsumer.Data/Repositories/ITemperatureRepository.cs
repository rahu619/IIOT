using IIOT.MessageConsumer.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IIOT.MessageConsumer.Data.Repositories
{
    public interface ITemperatureRepository
    {
        Task AddTemperatureAsync(Temperature temperatureValue);

        Task<IEnumerable<Temperature>> GetAllTemperaturesAsync();
    }
}
