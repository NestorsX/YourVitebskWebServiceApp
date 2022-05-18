using System;
using System.Collections.Generic;
using System.Linq;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.Repositories
{
    public class NewsRepository : IRepository<News>
    {
        private readonly YourVitebskDBContext _context;
        private bool _disposed = false;

        public NewsRepository(YourVitebskDBContext context)
        {
            _context = context;
        }

        public IEnumerable<IViewModel> Get()
        {
            return _context.News.ToList().OrderByDescending(x => x.NewsId);
        }

        public News Get(int id)
        {
            return _context.News.FirstOrDefault(x => x.NewsId == id);
        }

        public void Create(News news)
        {
            _context.News.Add(news);
            _context.SaveChanges();
        }

        public void Update(News news)
        {
            _context.News.Update(news);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            _context.News.Remove(Get(id));
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
