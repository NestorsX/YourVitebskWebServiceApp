using System;
using System.Collections.Generic;
using System.Linq;
using YourVitebskWebServiceApp.Helpers;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.Repositories
{
    public class CafeTypesRepository : IRepository<CafeType>
    {
        private readonly YourVitebskDBContext _context;
        private readonly RolePermissionManager _roleManager;
        private bool _disposed = false;

        public CafeTypesRepository(YourVitebskDBContext context)
        {
            _context = context;
            _roleManager = new RolePermissionManager(_context);
        }

        public bool CheckRolePermission(string userEmail, string permission)
        {
            return _roleManager.HasPermission(userEmail, permission);
        }

        public IEnumerable<IViewModel> Get()
        {
            return _context.CafeTypes.ToList();
        }

        public CafeType Get(int id)
        {
            return _context.CafeTypes.FirstOrDefault(x => x.CafeTypeId == id);
        }

        public void Create(CafeType cafeType)
        {
            _context.CafeTypes.Add(cafeType);
            _context.SaveChanges();
        }

        public void Update(CafeType cafeType)
        {
            _context.CafeTypes.Update(cafeType);
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
