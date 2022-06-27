using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using YourVitebskWebServiceApp.Helpers;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.Repositories
{
    public class PosterTypesRepository : IPosterTypeRepository
    {
        private readonly YourVitebskDBContext _context;
        private readonly RolePermissionManager _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private bool _disposed = false;

        public PosterTypesRepository(YourVitebskDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _roleManager = new RolePermissionManager(_context);
            _httpContextAccessor = httpContextAccessor;
        }

        public bool CheckRolePermission(string permission)
        {
            return _roleManager.HasPermission(_httpContextAccessor.HttpContext.User.Identity.Name, permission);
        }

        public IEnumerable<PosterType> Get()
        {
            return _context.PosterTypes.ToList();
        }

        public PosterType Get(int id)
        {
            return _context.PosterTypes.FirstOrDefault(x => x.PosterTypeId == id);
        }

        public void Create(PosterType posterType)
        {
            _context.PosterTypes.Add(posterType);
            _context.SaveChanges();
        }

        public void Update(PosterType posterType)
        {
            _context.PosterTypes.Update(posterType);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            _context.PosterTypes.Remove(Get(id));
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
