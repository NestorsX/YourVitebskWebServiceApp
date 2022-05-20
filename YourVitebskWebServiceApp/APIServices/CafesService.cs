﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourVitebskWebServiceApp.APIServiceInterfaces;

namespace YourVitebskWebServiceApp.APIServices
{
    public class CafesService : IService<APIModels.Cafe>
    {
        private readonly YourVitebskDBContext _context;

        public CafesService(YourVitebskDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<APIModels.Cafe>> GetAll()
        {
            IEnumerable<APIModels.Cafe> result = new List<APIModels.Cafe>();
            IEnumerable<Models.Cafe> cafes = (await _context.Cafes.ToListAsync()).OrderBy(x => x.Rating).ThenBy(x => x.CafeId);
            foreach (Models.Cafe cafe in cafes)
            {
                result = result.Append(new APIModels.Cafe()
                {
                    CafeId = (int)cafe.CafeId,
                    CafeType = (await _context.CafeTypes.FirstAsync(x => x.CafeTypeId == cafe.CafeTypeId)).Name,
                    Title = cafe.Title,
                    Description = cafe.Description,
                    WorkingTime = cafe.WorkingTime,
                    Address = cafe.Address,
                    ExternalLink = cafe.ExternalLink,
                    Rating = cafe.Rating == null ? "Без рейтинга" : cafe.Rating.ToString(),
                });
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
                Rating = cafe.Rating == null ? "Без рейтинга" : cafe.Rating.ToString(),
            };

            return result;
        }
    }
}
