using System.Collections.Generic;
using System.Threading.Tasks;
using YourVitebskWebServiceApp.APIModels;

namespace YourVitebskWebServiceApp.APIServiceInterfaces
{
    public interface IBusService
    {
        public Task<IEnumerable<Models.Bus>> GetAllBuses();
        public Task<IEnumerable<BusRoute>> GetBusRoutes(int busId);
        public Task<BusShedule> GetBusShedule(BusDTO busDTO);
    }
}
