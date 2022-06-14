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
    public class CafesService : IService<APIModels.Cafe>
    {
        private readonly YourVitebskDBContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public CafesService(YourVitebskDBContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<IEnumerable<APIModels.Cafe>> GetAll()
        {
            var result = new List<APIModels.Cafe>();
            IEnumerable<Models.Cafe> cafes = (await _context.Cafes.ToListAsync()).OrderByDescending(x => x.CafeId);
            foreach (Models.Cafe cafe in cafes)
            {
                result.Add(await GetById((int)cafe.CafeId));
            }

            return result;
        }

        public async Task<APIModels.Cafe> GetById(int id)
        {
            Models.Cafe cafe = await _context.Cafes.FirstOrDefaultAsync(x => x.CafeId == id);
            if (cafe == null)
            {
                throw new ArgumentException("Не найдено");
            }

            var result = new APIModels.Cafe()
            {
                CafeId = (int)cafe.CafeId,
                CafeType = _context.CafeTypes.First(x => x.CafeTypeId == cafe.CafeTypeId).Name,
                Title = cafe.Title,
                Description = cafe.Description,
                WorkingTime = cafe.WorkingTime,
                Address = cafe.Address,
                ExternalLink = cafe.ExternalLink,
                RecommendCount = _context.Comments.Where(x => x.ServiceId == 1 && x.IsRecommend == true).Count(),
                UnrecommendCount = _context.Comments.Where(x => x.ServiceId == 1 && x.IsRecommend == false).Count(),
                TitleImage = Directory.GetFiles($"{_appEnvironment.WebRootPath}/images/cafes/{cafe.CafeId}").Select(x => Path.GetFileName(x)).First(),
                Images = Directory.GetFiles($"{_appEnvironment.WebRootPath}/images/cafes/{cafe.CafeId}").Select(x => Path.GetFileName(x))
            };

            return result;
        }
    }
}
