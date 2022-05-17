using System;
using System.Collections.Generic;
using System.Linq;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;
using YourVitebskWebServiceApp.ViewModels;

namespace YourVitebskWebServiceApp.Repositories
{
    public class PostersRepository : IRepository<Models.Poster>
    {
        private readonly YourVitebskDBContext _context;
        private bool _disposed = false;

        public PostersRepository(YourVitebskDBContext context)
        {
            _context = context;
        }

        public IEnumerable<IViewModel> Get()
        {
            IEnumerable<ViewModels.Poster> result = new List<ViewModels.Poster>();
            IEnumerable<Models.Poster> posters = _context.Posters.ToList().OrderBy(x => x.PosterId);
            foreach (Models.Poster poster in posters)
            {
                result = result.Append(new ViewModels.Poster()
                {
                    PosterId = (int)poster.PosterId,
                    PosterType = _context.PosterTypes.First(x => x.PosterTypeId == poster.PosterTypeId).Name,
                    Title = poster.Title,
                    Description = poster.Description,
                    DateTime = ((DateTime)poster.DateTime).ToString("yyyy-MM-dd HH:mm"),
                    Address = poster.Address,
                    ExternalLink = poster.ExternalLink,
                });
            }

            return result;
        }

        public Models.Poster Get(int id)
        {
            return _context.Posters.FirstOrDefault(x => x.PosterId == id);
        }

        public void Create(Models.Poster poster)
        {
            _context.Posters.Add(poster);
            _context.SaveChanges();
        }

        public void Update(Models.Poster poster)
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
