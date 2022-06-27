using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using YourVitebskWebServiceApp.Helpers;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.Repositories
{
    public class CafeTypesRepository : ICafeTypeRepository
    {
        private readonly YourVitebskDBContext _context;
        private readonly RolePermissionManager _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private bool _disposed = false;

        public CafeTypesRepository(YourVitebskDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _roleManager = new RolePermissionManager(_context);
            _httpContextAccessor = httpContextAccessor;
        }

        public bool CheckRolePermission(string permission)
        {
            return _roleManager.HasPermission(_httpContextAccessor.HttpContext.User.Identity.Name, permission);
        }

        public IEnumerable<CafeType> Get()
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
