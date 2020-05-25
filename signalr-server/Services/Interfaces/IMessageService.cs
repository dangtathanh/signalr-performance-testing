using System.Threading.Tasks;

namespace signalr.server.Services
{
    public interface IMessageService
    {
        Task SendAllAsync(int index);
    }
}
