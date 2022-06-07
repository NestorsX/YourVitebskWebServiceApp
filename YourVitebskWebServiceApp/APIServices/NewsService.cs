using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YourVitebskWebServiceApp.APIServiceInterfaces;

namespace YourVitebskWebServiceApp.APIServices
{
    public class NewsService : IService<APIModels.News>
    {
        private readonly YourVitebskDBContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public NewsService(YourVitebskDBContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<IEnumerable<APIModels.News>> GetAll()
        {
            IEnumerable<APIModels.News> result = new List<APIModels.News>();
            IEnumerable<Models.News> newsList = (await _context.News.ToListAsync()).OrderByDescending(x => x.NewsId);
            foreach (Models.News news in newsList)
            {
                result = result.Append(new APIModels.News()
                {
                    NewsId = (int)news.NewsId,
                    Title = news.Title,
                    Description = news.Description,
                    ExternalLink = news.ExternalLink,
                    TitleImage = Directory.GetFiles($"{_appEnvironment.WebRootPath}/images/news/{news.NewsId}").Select(x => Path.GetFileName(x)).First(),
                    Images = Directory.GetFiles($"{_appEnvironment.WebRootPath}/images/news/{news.NewsId}").Select(x => Path.GetFileName(x))
                });
            }

            return result;
        }

        public async Task<APIModels.News> GetById(int id)
        {
            Models.News news = await _context.News.FirstOrDefaultAsync(x => x.NewsId == id);
            if (news == null)
            {
                throw new ArgumentException("Не найдено");
            }

            var result = new APIModels.News()
            {
                NewsId = (int)news.NewsId,
                Title = news.Title,
                Description = news.Description,
                ExternalLink = news.ExternalLink,
                TitleImage = Directory.GetFiles($"{_appEnvironment.WebRootPath}/images/news/{news.NewsId}").Select(x => Path.GetFileName(x)).First(),
                Images = Directory.GetFiles($"{_appEnvironment.WebRootPath}/images/news/{news.NewsId}").Select(x => Path.GetFileName(x))
            };

            return result;
        }
    }
}
