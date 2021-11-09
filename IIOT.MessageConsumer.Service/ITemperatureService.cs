using IIOT.MessageConsumer.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IIOT.MessageConsumer.Service
{
    public interface ITemperatureService
    {
        Task<IEnumerable<Temperature>> GetAllTemperatures();

        Task AddTemperature(Temperature temperature);
    }
}
