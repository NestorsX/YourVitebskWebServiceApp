using System;
using System.Collections.Generic;
using System.Linq;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.Repositories
{
    public class CafeTypesRepository : IRepository<CafeType>
    {
        private readonly YourVitebskDBContext _context;
        private bool _disposed = false;

        public CafeTypesRepository(YourVitebskDBContext context)
        {
            _context = context;
        }

        public IEnumerable<IViewModel> Get()
        {
            return _context.CafeTypes.ToList().OrderBy(x => x.CafeTypeId);
        }

        public CafeType Get(int id)
        {
            return _context.CafeTypes.FirstOrDefault(x => x.CafeTypeId == id);
        }

        public void Create(CafeType CafeType)
        {
            _context.CafeTypes.Add(CafeType);
            _context.SaveChanges();
        }

        public void Update(CafeType CafeType)
        {
            _context.CafeTypes.Update(CafeType);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            _context.CafeTypes.Remove(Get(id));
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
