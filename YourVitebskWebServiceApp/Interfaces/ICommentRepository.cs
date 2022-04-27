using System.Collections.Generic;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.Interfaces
{
    public interface ICommentRepository
    {
        IEnumerable<Comment> Get();

        Comment Get(int id);

        void Delete(int id);
    }
}
