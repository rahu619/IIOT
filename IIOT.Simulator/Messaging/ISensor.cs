using System.Threading.Tasks;

namespace IIOT.Simulator.Messaging
{
    public interface ISensor
    {
        Task Generate();

        delegate Task MessageReceivedHandler(string temperature);

        event MessageReceivedHandler MessageReceived;
    }
}
