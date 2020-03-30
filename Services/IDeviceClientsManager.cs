
using System.Threading;
using System.Threading.Tasks;

namespace D2C.Services{

    public interface IDeviceClientsManager
    {
        Task InitiateConnectionsAsync(CancellationToken cancellationToken);

        Task SendMessageAsync(string deviceId, string message);

        Task ShutDown();
    }
}