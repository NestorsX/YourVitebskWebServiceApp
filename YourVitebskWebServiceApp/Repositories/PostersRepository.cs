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
    public class PostersRepository : IImageRepository<Poster>
    {
        private readonly YourVitebskDBContext _context;
        private readonly ImageService _imageService;
        private readonly RolePermissionManager _roleManager;
        private bool _disposed = false;

        public PostersRepository(YourVitebskDBContext context, IWebHostEnvironment appEnvironment)
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
            return _context.Posters.ToList().OrderBy(x => x.PosterId);
        }

        public IViewModel Get(int id)
        {
            return _context.Posters.FirstOrDefault(x => x.PosterId == id);
        }

        public void Create(Poster poster, IFormFileCollection uploadedFiles)
        {
            _context.Posters.Add(poster);
            _context.SaveChanges();
            _imageService.SaveImages("posters", (int)poster.PosterId, uploadedFiles);
        }

        public void Update(Poster poster, IFormFileCollection uploadedFiles)
        {
            _context.Posters.Update(poster);
            _context.SaveChanges();
            _imageService.SaveImages("posters", (int)poster.PosterId, uploadedFiles);
        }

        public void Delete(int id)
        {
            _context.Posters.Remove((Poster)Get(id));
            _context.SaveChanges();
            _imageService.DeleteImages("posters", id);
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
