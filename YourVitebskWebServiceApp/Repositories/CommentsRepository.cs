using System;
using System.Collections.Generic;
using System.Linq;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.Repositories
{
    public class CommentsRepository : ICommentRepository
    {
        private readonly YourVitebskDBContext _context;
        private bool _disposed = false;

        public CommentsRepository(YourVitebskDBContext context)
        {
            _context = context;
        }

        public IEnumerable<Comment> Get()
        {
            return _context.Comments.ToList().OrderBy(x => x.CommentId);
        }

        public Comment Get(int id)
        {
            return _context.Comments.FirstOrDefault(x => x.CommentId == id);
        }

        public void Delete(int id)
        {
            _context.Comments.Remove(Get(id));
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
