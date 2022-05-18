using System;
using System.Collections.Generic;
using System.Linq;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.Repositories
{
    public class BusStopsRepository : IRepository<BusStop>
    {
        private readonly YourVitebskDBContext _context;
        private bool _disposed = false;

        public BusStopsRepository(YourVitebskDBContext context)
        {
            _context = context;
        }

        public IEnumerable<IViewModel> Get()
        {
            return _context.BusStops.ToList().OrderBy(x => x.BusStopId);
        }

        public BusStop Get(int id)
        {
            return _context.BusStops.FirstOrDefault(x => x.BusStopId == id);
        }

        public void Create(BusStop role)
        {
            _context.BusStops.Add(role);
            _context.SaveChanges();
        }

        public void Update(BusStop role)
        {
            _context.BusStops.Update(role);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            _context.BusStops.Remove(Get(id));
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
