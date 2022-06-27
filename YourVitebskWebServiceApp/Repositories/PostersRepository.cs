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
    public class PostersRepository : IPosterRepository
    {
        private readonly YourVitebskDBContext _context;
        private readonly ImageService _imageService;
        private readonly RolePermissionManager _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private bool _disposed = false;

        public PostersRepository(YourVitebskDBContext context, IWebHostEnvironment appEnvironment, IHttpContextAccessor httpContextAccessor)
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

        public IEnumerable<PosterViewModel> Get()
        {
            var result = new List<PosterViewModel>();
            IEnumerable<Poster> posters = _context.Posters.ToList();
            foreach (var poster in posters)
            {
                result.Add(new PosterViewModel()
                {
                    PosterId = (int)poster.PosterId,
                    PosterTypeId = poster.PosterTypeId,
                    PosterType = _context.PosterTypes.First(x => x.PosterTypeId == poster.PosterTypeId).Name,
                    Title = poster.Title,
                    Description = poster.Description,
                    DateTime = poster.DateTime,
                    Address = poster.Address,
                    ExternalLink = poster.ExternalLink
                });
            }

            return result.ToList();
        }

        public Poster Get(int id)
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
            _context.Posters.Remove(Get(id));
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
