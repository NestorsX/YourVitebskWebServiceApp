using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YourVitebskWebServiceApp.APIModels;
using YourVitebskWebServiceApp.APIServiceInterfaces;

namespace YourVitebskWebServiceApp.APIServices
{
    public class CommentsService : ICommentService
    {
        private readonly YourVitebskDBContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public CommentsService(YourVitebskDBContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<IEnumerable<Comment>> GetAll(int serviceId, int itemId)
        {
            var result = new List<Comment>();
            IEnumerable<Models.Comment> comments = (await _context.Comments.Where(x => x.ServiceId == serviceId && x.ItemId == itemId).ToListAsync()).OrderByDescending(x => x.PublishDate).ThenByDescending(x => x.CommentId);
            foreach (Models.Comment comment in comments)
            {
                var user = await _context.Users.FirstAsync(x => x.UserId == comment.UserId);
                string image = "";
                if (Directory.Exists($"{_appEnvironment.WebRootPath}/images/users/{user.UserId}"))
                {
                    image = Directory.GetFiles($"{_appEnvironment.WebRootPath}/images/users/{user.UserId}").Select(x => Path.GetFileName(x)).First();
                }

                result.Add(new Comment()
                {
                    CommentId = (int)comment.CommentId,
                    UserId = comment.UserId,
                    Image = image,
                    UserFirstName = user.FirstName,
                    IsRecommend = comment.IsRecommend ? "Рекомендует" : "Не рекомендует",
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
