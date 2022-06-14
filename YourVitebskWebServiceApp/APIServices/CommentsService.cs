using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using YourVitebskWebServiceApp.APIModels;
using YourVitebskWebServiceApp.APIServiceInterfaces;

namespace YourVitebskWebServiceApp.APIServices
{
    public class CommentsService : ICommentService
    {
        private readonly YourVitebskDBContext _context;

        public CommentsService(YourVitebskDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetAll(int serviceId, int itemId)
        {
            var result = new List<Comment>();
            IEnumerable<Models.Comment> comments = (await _context.Comments.Where(x => x.ServiceId == serviceId && x.ItemId == itemId).ToListAsync()).OrderByDescending(x => x.PublishDate);
            foreach (Models.Comment comment in comments)
            {
                result.Add(new Comment()
                {
                    CommentId = (int)comment.CommentId,
                    UserId = comment.UserId,
                    UserFirstName = (await _context.Users.FirstAsync(x => x.UserId == comment.UserId)).FirstName,
                    IsRecommend = comment.IsRecommend,
                    Message = comment.Message,
                    PublishDate = comment.PublishDate.ToString("D", new CultureInfo("ru-RU")) + comment.PublishDate.ToString(" HH:mm")
                });
            }

            return result;
        }

        public async Task AddComment(CommentDTO newComment)
        {
            if (!_context.Users.Any(x => x.UserId == newComment.UserId))
            {
                throw new ArgumentException("Неверный ID пользователя");
            }

            switch (newComment.ServiceId)
            {
                case 1:
                    if (!_context.Cafes.Any(x => x.CafeId == newComment.ItemId))
                    {
                        throw new ArgumentException("Неверный ID заведения");
                    }

                    break;
                case 2:
                    if (!_context.Posters.Any(x => x.PosterId == newComment.ItemId))
                    {
                        throw new ArgumentException("Неверный ID афиши");
                    }

                    break;
                default:
                    throw new ArgumentException("Неверный ID сервиса");
            }

            _context.Comments.Add(new Models.Comment
            {
                CommentId = null,
                UserId = newComment.UserId,
                ServiceId = newComment.ServiceId,
                ItemId = newComment.ItemId,
                IsRecommend = newComment.IsRecommend,
                Message = newComment.Message,
                PublishDate = newComment.PublishDate
            });

            await _context.SaveChangesAsync();
        }
    }
}
