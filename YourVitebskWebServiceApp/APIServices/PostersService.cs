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
    public class PostersService : IService<APIModels.Poster>
    {
        private readonly YourVitebskDBContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public PostersService(YourVitebskDBContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
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
                    DateTime = poster.DateTime,
                    Address = poster.Address,
                    ExternalLink = poster.ExternalLink,
                    TitleImage = Directory.GetFiles($"{_appEnvironment.WebRootPath}/images/posters/{poster.PosterId}").Select(x => Path.GetFileName(x)).First(),
                    Images = Directory.GetFiles($"{_appEnvironment.WebRootPath}/images/posters/{poster.PosterId}").Select(x => Path.GetFileName(x))
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
                DateTime = poster.DateTime,
                Address = poster.Address,
                ExternalLink = poster.ExternalLink,
                TitleImage = Directory.GetFiles($"{_appEnvironment.WebRootPath}/images/posters/{poster.PosterId}").Select(x => Path.GetFileName(x)).First(),
                Images = Directory.GetFiles($"{_appEnvironment.WebRootPath}/images/posters/{poster.PosterId}").Select(x => Path.GetFileName(x))

            };

            return result;
        }
    }
}
