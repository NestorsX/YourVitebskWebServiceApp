using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourVitebskWebServiceApp.APIServiceInterfaces;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.APIServices
{
    public class NewsService : IService<News>
    {
        private readonly YourVitebskDBContext _context;

        public NewsService(YourVitebskDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<News>> GetAll()
        {

            return (await _context.News.ToListAsync()).OrderByDescending(x => x.NewsId);
        }

        public async Task<News> GetById(int id)
        {
            News news = await _context.News.FirstOrDefaultAsync(x => x.NewsId == id);
            if (news == null)
            {
                throw new ArgumentException("Не найдено");
            }

            return news;
        }
    }
}
