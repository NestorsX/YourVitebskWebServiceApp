using System;
using System.Collections.Generic;
using System.Linq;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;
using YourVitebskWebServiceApp.ViewModels;

namespace YourVitebskWebServiceApp.Repositories
{
    public class PostersRepository : IRepository<Poster>
    {
        private readonly YourVitebskDBContext _context;
        private bool _disposed = false;

        public PostersRepository(YourVitebskDBContext context)
        {
            _context = context;
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

        public Poster Get(int id)
        {
            return _context.Posters.FirstOrDefault(x => x.PosterId == id);
        }

        public void Create(Poster poster)
        {
            _context.Posters.Add(poster);
            _context.SaveChanges();
        }

        public void Update(Poster poster)
        {
            _context.Posters.Update(poster);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            _context.Posters.Remove(Get(id));
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
