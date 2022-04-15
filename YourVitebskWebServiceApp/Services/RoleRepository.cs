using System.Collections.Generic;
using System.Linq;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.Services
{
    public class RoleRepository : IRepository<Role>
    {
        private readonly YourVitebskDBContext _context;

        public RoleRepository(YourVitebskDBContext context)
        {
            _context = context;
        }

        public IEnumerable<Role> Get()
        {
            return _context.Roles.ToList().OrderBy(x => x.RoleId);
        }

        public Role Get(int id)
        {
            return _context.Roles.FirstOrDefault(x => x.RoleId == id);
        }

        public void Create(Role role)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
        }

        public void Update(Role role)
        {
            _context.Roles.Update(role);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            _context.Roles.Remove(Get(id));
            _context.SaveChanges();
        }
    }
}
