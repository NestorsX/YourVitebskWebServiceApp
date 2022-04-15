using System.Collections.Generic;
using System.Linq;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.Services
{
    public class ServiceRepository : IRepository<Service>
    {
        private readonly YourVitebskDBContext _context;

        public ServiceRepository(YourVitebskDBContext context)
        {
            _context = context;
        }

        public IEnumerable<Service> Get()
        {
            return _context.Services.ToList().OrderBy(x => x.ServiceId);
        }

        public Service Get(int id)
        {
            return _context.Services.FirstOrDefault(x => x.ServiceId == id);
        }

        public void Create(Service service)
        {
            _context.Services.Add(service);
            _context.SaveChanges();
        }

        public void Update(Service service)
        {
            _context.Services.Update(service);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            _context.Services.Remove(Get(id));
            _context.SaveChanges();
        }
    }
}
