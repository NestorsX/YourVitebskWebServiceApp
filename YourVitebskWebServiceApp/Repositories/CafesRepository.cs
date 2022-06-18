using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using YourVitebskWebServiceApp.Helpers;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.Repositories
{
    public class CafesRepository : IImageRepository<Cafe>
    {
        private readonly YourVitebskDBContext _context;
        private readonly ImageService _imageService;
        private readonly RolePermissionManager _roleManager;
        private bool _disposed = false;

        public CafesRepository(YourVitebskDBContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _imageService = new ImageService(appEnvironment);
            _roleManager = new RolePermissionManager(_context);
        }

        public bool CheckRolePermission(string userEmail, string permission)
        {
            return _roleManager.HasPermission(userEmail, permission);
        }

        public IEnumerable<IViewModel> Get()
        {
            return _context.Cafes.ToList();
        }

        public IViewModel Get(int id)
        {
            return _context.Cafes.FirstOrDefault(x => x.CafeId == id);
        }

        public void Create(Cafe cafe, IFormFileCollection uploadedFiles)
        {
            _context.Cafes.Add(cafe);
            _context.SaveChanges();
            _imageService.SaveImages("cafes", (int)cafe.CafeId, uploadedFiles);
        }

        public void Update(Cafe cafe, IFormFileCollection uploadedFiles)
        {
            _context.Cafes.Update(cafe);
            _context.SaveChanges();
            _imageService.SaveImages("cafes", (int)cafe.CafeId, uploadedFiles);
        }

        public void Delete(int id)
        {
            Cafe cafe = _context.Cafes.FirstOrDefault(x => x.CafeId == id);
            _context.Cafes.Remove(cafe);
            _context.SaveChanges();
            _imageService.DeleteImages("cafes", id);
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
