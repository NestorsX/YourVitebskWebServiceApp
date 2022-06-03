using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;
using YourVitebskWebServiceApp.ViewModels;

namespace YourVitebskWebServiceApp.Repositories
{
    public class CafesRepository : IImageRepository<Cafe>
    {
        private readonly YourVitebskDBContext _context;
        private readonly ImageService _imageService;
        private bool _disposed = false;

        public CafesRepository(YourVitebskDBContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _imageService = new ImageService(appEnvironment);
        }

        public IEnumerable<IViewModel> Get()
        {
            IEnumerable<CafeViewModel> result = new List<CafeViewModel>();
            IEnumerable<Cafe> cafes = _context.Cafes.ToList();
            foreach (Cafe cafe in cafes)
            {
                result = result.Append(new CafeViewModel()
                {
                    CafeId = (int)cafe.CafeId,
                    CafeType = _context.CafeTypes.First(x => x.CafeTypeId == cafe.CafeTypeId).Name,
                    Title = cafe.Title,
                    Description = cafe.Description,
                    WorkingTime = cafe.WorkingTime,
                    Address = cafe.Address,
                    ExternalLink = cafe.ExternalLink,
                });
            }

            return result;
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
            _context.Cafes.Remove((Cafe)Get(id));
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
