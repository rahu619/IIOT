using IIOT.MessageConsumer.Data.Entities;
using IIOT.MessageConsumer.Data.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IIOT.MessageConsumer.Data.Repositories
{
    public class TemperatureRepository : Repository<Temperature>, ITemperatureRepository
    {
        public TemperatureRepository(MessageConsumerDbContext dbContext) : base(dbContext) { }

        public async Task AddTemperatureAsync(Temperature temperatureValue)
        {
            await AddAsync(temperatureValue);
        }

        public async Task<IEnumerable<Temperature>> GetAllTemperaturesAsync() => await GetAllAsync();

        public async Task<Temperature> GetTemperatureByIdAsync(Guid Id) => await GetByIDAsync(Id);
    }
}
