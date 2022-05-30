using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using YourVitebskWebServiceApp.APIModels;
using YourVitebskWebServiceApp.APIServiceInterfaces;
using System;

namespace YourVitebskWebServiceApp.APIServices
{
    public class BusesService : IBusService
    {
        private readonly YourVitebskDBContext _context;

        public BusesService(YourVitebskDBContext context)
        {
            _context = context;
        }

        private async Task<IEnumerable<Models.BusStop>> GetBusStopsByRoute(int busId, int routeNumber)
        {
            IEnumerable<Models.BusShedule> busShedules = await _context.BusShedules.Where(x => x.BusId == busId && x.BusRoute == routeNumber).OrderBy(x => x.BusStopNumber).ToListAsync();
            var busStops = new List<Models.BusStop>();
            foreach (var busShedule in busShedules)
            {
                busStops.Add(await _context.BusStops.FirstAsync(x => x.BusStopId == busShedule.BusStopId));
            }

            return busStops;
        }

        private string GetRouteName(IEnumerable<Models.BusStop> busStops)
        {
            return $"{busStops.First().Name} - {busStops.Last().Name}";
        }

        public async Task<IEnumerable<Models.Bus>> GetAllBuses()
        {
            return await _context.Buses.ToListAsync();
        }

        public async Task<IEnumerable<BusRoute>> GetBusRoutes(int busId)
        {
            Models.Bus bus = await _context.Buses.FirstAsync(x => x.BusId == busId);
            if (bus == null)
            {
                throw new ArgumentException("Не найдено");
            }

            IEnumerable<Models.BusShedule> busShedules = await _context.BusShedules.Where(x => x.BusId == busId).ToListAsync();
            var busStopsForFirstRoute = await GetBusStopsByRoute(busId, 1);
            var busStopsForSecondRoute = await GetBusStopsByRoute(busId, 2);
            IEnumerable<BusRoute> busRoutes = new List<BusRoute>
            {
                new BusRoute
                {
                    BusId = busId,
                    BusNumber = bus.Number,
                    BusRouteNumber = 1,
                    BusRouteName = GetRouteName(busStopsForFirstRoute),
                    BusStops = busStopsForFirstRoute
                },
                new BusRoute
                {
                    BusId = busId,
                    BusNumber = bus.Number,
                    BusRouteNumber = 2,
                    BusRouteName = GetRouteName(busStopsForSecondRoute),
                    BusStops = busStopsForSecondRoute
                }
            };

            return busRoutes;
        }

        public async Task<BusShedule> GetBusShedule(BusDTO busDTO)
        {
            Models.Bus bus = await _context.Buses.FirstOrDefaultAsync(x => x.BusId == busDTO.BusId);
            if (bus == null)
            {
                throw new ArgumentException("Не найдено");
            }

            Models.BusStop busStop = await _context.BusStops.FirstOrDefaultAsync(x => x.BusStopId == busDTO.BusStopId);
            if (busStop == null)
            {
                throw new ArgumentException("Не найдено");
            }

            Models.BusShedule busShedules = await _context.BusShedules.FirstOrDefaultAsync(x => x.BusId == busDTO.BusId
                                                                                            && x.BusStopId == busDTO.BusStopId
                                                                                            && x.BusRoute == busDTO.BusRoute);
            if (busShedules == null)
            {
                throw new ArgumentException("Не найдено");
            }

            return new BusShedule
            {
                BusNumber = bus.Number,
                BusStopName = busStop.Name,
                BusRouteName = GetRouteName(await GetBusStopsByRoute(busDTO.BusId, busDTO.BusRoute)),
                WorkDayShedule = busShedules.WorkDayShedule,
                DayOffShedule = busShedules.DayOffShedule
            };
        }
    }
}
