using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;
using YourVitebskWebServiceApp.ViewModels;

namespace YourVitebskWebServiceApp.Repositories
{
    public class BusShedulesRepository : IRepository<BusShedule>
    {
        private readonly YourVitebskDBContext _context;
        private bool _disposed = false;

        public BusShedulesRepository(YourVitebskDBContext context)
        {
            _context = context;
        }

        public IEnumerable<IViewModel> Get()
        {
            IEnumerable<BusSheduleViewModel> result = new List<BusSheduleViewModel>();
            IEnumerable<BusShedule> busShedules = _context.BusShedules.ToList();
            foreach (BusShedule busShedule in busShedules)
            {
                result = result.Append(new BusSheduleViewModel()
                {
                    BusSheduleId = (int)busShedule.BusSheduleId,
                    BusNumber = _context.Buses.First(x => x.BusId == busShedule.BusId).Number,
                    BusStopName = _context.BusStops.First(x => x.BusStopId == busShedule.BusStopId).Name,
                    BusRoute = busShedule.BusRoute,
                    BusStopNumber = busShedule.BusStopNumber,
                    WorkDayShedule = busShedule.WorkDayShedule,
                    DayOffShedule = busShedule.DayOffShedule
                });
            }

            return result;
        }

        public BusShedule Get(int id)
        {
            return _context.BusShedules.FirstOrDefault(x => x.BusSheduleId == id);
        }

        public void Create(BusShedule busShedule)
        {
            _context.BusShedules.Add(busShedule);
            _context.SaveChanges();
        }

        public void Update(BusShedule busShedule)
        {
            _context.BusShedules.Update(busShedule);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            _context.BusShedules.Remove(Get(id));
            _context.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
