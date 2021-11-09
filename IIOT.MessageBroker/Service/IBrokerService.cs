using System.Threading.Tasks;

namespace IIOT.MessageBroker.Service
{
    public interface IBrokerService
    {
        Task Start();
        Task Stop();
    }
}
