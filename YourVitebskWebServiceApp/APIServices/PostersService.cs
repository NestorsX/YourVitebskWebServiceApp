using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourVitebskWebServiceApp.APIServiceInterfaces;

namespace YourVitebskWebServiceApp.APIServices
{
    public class PostersService : IService<APIModels.Poster>
    {
        private readonly YourVitebskDBContext _context;

        public PostersService(YourVitebskDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<APIModels.Poster>> GetAll()
        {
            IEnumerable<APIModels.Poster> result = new List<APIModels.Poster>();
            IEnumerable<Models.Poster> posters = (await _context.Posters.ToListAsync()).OrderBy(x => x.DateTime);
            foreach (Models.Poster poster in posters)
            {
                result = result.Append(new APIModels.Poster()
                {
                    PosterId = (int)poster.PosterId,
                    PosterType = (await _context.PosterTypes.FirstAsync(x => x.PosterTypeId == poster.PosterTypeId)).Name,
                    Title = poster.Title,
                    Description = poster.Description,
                    DateTime = ((DateTime)poster.DateTime).ToString("f"),
                    Address = poster.Address,
                    ExternalLink = poster.ExternalLink,
                });
            }

            return result;
        }

        public async Task<APIModels.Poster> GetById(int id)
        {
            Models.Poster poster = await _context.Posters.FirstOrDefaultAsync(x => x.PosterId == id);
            if (poster == null)
            {
                throw new ArgumentException("Не найдено");
            }

            var result = new APIModels.Poster()
            {
                PosterId = (int)poster.PosterId,
                PosterType = _context.PosterTypes.First(x => x.PosterTypeId == poster.PosterTypeId).Name,
                Title = poster.Title,
                Description = poster.Description,
                DateTime = ((DateTime)poster.DateTime).ToString("f"),
                Address = poster.Address,
                ExternalLink = poster.ExternalLink,
            };

            return result;
        }
    }
}
