using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using YourVitebskWebServiceApp.APIServiceInterfaces;

namespace YourVitebskWebServiceApp.APIServices
{
    public class VacanciesService : IService<APIModels.Vacancy>
    {
        private readonly YourVitebskDBContext _context;

        public VacanciesService(YourVitebskDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<APIModels.Vacancy>> GetAll()
        {
            var result = new List<APIModels.Vacancy>();
            IEnumerable<Models.Vacancy> vacancies = (await _context.Vacancies.ToListAsync()).OrderByDescending(x => x.PublishDate).ThenByDescending(x => x.VacancyId);
            foreach (var vacancy in vacancies)
            {
                result.Add(await GetById((int)vacancy.VacancyId));
            }

            return result;
        }

        public async Task<APIModels.Vacancy> GetById(int id)
        {
            Models.Vacancy vacancy = await _context.Vacancies.FirstOrDefaultAsync(x => x.VacancyId == id);
            if (vacancy == null)
            {
                throw new ArgumentException("Не найдено");
            }

            var result = new APIModels.Vacancy()
            {
                VacancyId = (int)vacancy.VacancyId,
                Title = vacancy.Title,
                Description = vacancy.Description,
                Requirements = vacancy.Requirements,
                Conditions = vacancy.Conditions,
                Salary = vacancy.Salary,
                CompanyName = vacancy.CompanyName,
                Contacts = vacancy.Contacts,
                Address = vacancy.Address,
                PublishDate = vacancy.PublishDate.ToString("D", new CultureInfo("ru-RU"))
            };

            return result;
        }
    }
}
