using System.Collections.Generic;
using System.Threading.Tasks;
using YourVitebskWebServiceApp.APIModels;

namespace YourVitebskWebServiceApp.APIServiceInterfaces
{
    public interface ICommentService
    {
        public Task<IEnumerable<Comment>> GetAll(int serviceId, int itemId);
        public Task AddComment(CommentDTO newComment);
    }
}
