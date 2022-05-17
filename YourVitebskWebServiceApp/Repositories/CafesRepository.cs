using System;
using System.Collections.Generic;
using System.Linq;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;
using YourVitebskWebServiceApp.ViewModels;

namespace YourVitebskWebServiceApp.Repositories
{
    public class CafesRepository : IRepository<Models.Cafe>
    {
        private readonly YourVitebskDBContext _context;
        private bool _disposed = false;

        public CafesRepository(YourVitebskDBContext context)
        {
            _context = context;
        }

        public IEnumerable<IViewModel> Get()
        {
            IEnumerable<ViewModels.Cafe> result = new List<ViewModels.Cafe>();
            IEnumerable<Models.Cafe> cafes = _context.Cafes.ToList().OrderBy(x => x.CafeId);
            foreach (Models.Cafe cafe in cafes)
            {
                result = result.Append(new ViewModels.Cafe()
                {
                    CafeId = (int)cafe.CafeId,
                    CafeType = _context.CafeTypes.First(x => x.CafeTypeId == cafe.CafeTypeId).Name,
                    Title = cafe.Title,
                    Description = cafe.Description,
                    WorkingTime = cafe.WorkingTime,
                    Address = cafe.Address,
                    ExternalLink = cafe.ExternalLink,
                    Rating = cafe.Rating,
                });
            }

            return result;
        }

        public Models.Cafe Get(int id)
        {
            return _context.Cafes.FirstOrDefault(x => x.CafeId == id);
        }

        public void Create(Models.Cafe cafe)
        {
            _context.Cafes.Add(cafe);
            _context.SaveChanges();
        }

        public void Update(Models.Cafe cafe)
        {
            _context.Cafes.Update(cafe);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            _context.Cafes.Remove(Get(id));
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
