using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using YourVitebskWebServiceApp.Helpers;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;
using YourVitebskWebServiceApp.ViewModels;

namespace YourVitebskWebServiceApp.Repositories
{
    public class CafesRepository : ICafeRepository
    {
        private readonly YourVitebskDBContext _context;
        private readonly ImageService _imageService;
        private readonly RolePermissionManager _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private bool _disposed = false;

        public CafesRepository(YourVitebskDBContext context, IWebHostEnvironment appEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _imageService = new ImageService(appEnvironment);
            _roleManager = new RolePermissionManager(_context);
            _httpContextAccessor = httpContextAccessor;
        }

        public bool CheckRolePermission(string permission)
        {
            return _roleManager.HasPermission(_httpContextAccessor.HttpContext.User.Identity.Name, permission);
        }

        public IEnumerable<CafeViewModel> Get()
        {
            var result = new List<CafeViewModel>();
            IEnumerable<Cafe> cafes = _context.Cafes.ToList();
            foreach (var cafe in cafes)
            {
                result.Add(new CafeViewModel()
                {
                    CafeId = (int)cafe.CafeId,
                    CafeTypeId = cafe.CafeTypeId,
                    CafeType = _context.CafeTypes.First(x => x.CafeTypeId == cafe.CafeTypeId).Name,
                    Title = cafe.Title,
                    Description = cafe.Description,
                    WorkingTime = cafe.WorkingTime,
                    Address = cafe.Address,
                    ExternalLink = cafe.ExternalLink
                });
            }

            return result.ToList();
        }

        public Cafe Get(int id)
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
