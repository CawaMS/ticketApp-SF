
namespace IStatelessBackendService.Interfaces
{
    using System.Threading.Tasks;
    using Microsoft.ServiceFabric.Services.Remoting;
    public interface IStatelessBackendService : IService
    {
        Task<long> GetCountAsync();
    }
}
