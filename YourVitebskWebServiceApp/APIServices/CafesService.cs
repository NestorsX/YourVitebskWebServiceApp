using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using YourVitebskWebServiceApp.APIServiceInterfaces;
using YourVitebskWebServiceApp.Models;
using YourVitebskWebServiceApp.ViewModels;

namespace YourVitebskWebServiceApp.APIServices
{
    public class CafesService : IService<CafeViewModel>
    {
        private readonly YourVitebskDBContext _context;

        public CafesService(YourVitebskDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CafeViewModel>> GetAll()
        {
            IEnumerable<CafeViewModel> result = new List<CafeViewModel>();
            IEnumerable<Cafe> cafes = (await _context.Cafes.ToListAsync()).OrderBy(x => x.Rating).ThenBy(x => x.CafeId);
            foreach (Cafe cafe in cafes)
            {
                result = result.Append(new CafeViewModel()
                {
                    CafeId = (int)cafe.CafeId,
                    CafeType = (await _context.CafeTypes.FirstAsync(x => x.CafeTypeId == cafe.CafeTypeId)).Name,
                    Title = cafe.Title,
                    Description = cafe.Description,
                    WorkingTime = cafe.WorkingTime,
                    Address = cafe.Address,
                    ExternalLink = cafe.ExternalLink,
                    Rating = cafe.Rating,
                });
            }

            return result;
        }

        public async Task<CafeViewModel> GetById(int id)
        {
            Cafe cafe = await _context.Cafes.FirstOrDefaultAsync(x => x.CafeId == id);
            var result = new CafeViewModel()
            {
                CafeId = (int)cafe.CafeId,
                CafeType = _context.CafeTypes.First(x => x.CafeTypeId == cafe.CafeTypeId).Name,
                Title = cafe.Title,
                Description = cafe.Description,
                WorkingTime = cafe.WorkingTime,
                Address = cafe.Address,
                ExternalLink = cafe.ExternalLink,
                Rating = cafe.Rating,
            };

            return result;
        }
    }
}
