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
    public class PostersRepository : IImageRepository<Poster>
    {
        private readonly YourVitebskDBContext _context;
        private readonly ImageService _imageService;
        private bool _disposed = false;

        public PostersRepository(YourVitebskDBContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _imageService = new ImageService(appEnvironment);
        }

        public IEnumerable<IViewModel> Get()
        {
            IEnumerable<PosterViewModel> result = new List<PosterViewModel>();
            IEnumerable<Poster> posters = _context.Posters.ToList().OrderBy(x => x.PosterId);
            foreach (Poster poster in posters)
            {
                result = result.Append(new PosterViewModel()
                {
                    PosterId = (int)poster.PosterId,
                    PosterType = _context.PosterTypes.First(x => x.PosterTypeId == poster.PosterTypeId).Name,
                    Title = poster.Title,
                    Description = poster.Description,
                    DateTime = poster.DateTime,
                    Address = poster.Address,
                    ExternalLink = poster.ExternalLink,
                });
            }

            return result;
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
