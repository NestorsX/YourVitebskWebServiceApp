using System;
using System.Collections.Generic;
using System.Linq;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.Repositories
{
    public class BusesRepository : IRepository<Bus>
    {
        private readonly YourVitebskDBContext _context;
        private bool _disposed = false;

        public BusesRepository(YourVitebskDBContext context)
        {
            _context = context;
        }

        public IEnumerable<IViewModel> Get()
        {
            return _context.Buses.ToList().OrderBy(x => x.BusId);
        }

        public Bus Get(int id)
        {
            return _context.Buses.FirstOrDefault(x => x.BusId == id);
        }

        public void Create(Bus role)
        {
            _context.Buses.Add(role);
            _context.SaveChanges();
        }

        public void Update(Bus role)
        {
            _context.Buses.Update(role);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            _context.Buses.Remove(Get(id));
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
