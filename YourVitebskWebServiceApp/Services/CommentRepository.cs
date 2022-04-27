using System.Collections.Generic;
using System.Linq;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.Services
{
    public class CommentRepository : ICommentRepository
    {
        private readonly YourVitebskDBContext _context;

        public CommentRepository(YourVitebskDBContext context)
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
    }
}
