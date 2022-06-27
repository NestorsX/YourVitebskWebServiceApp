using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using YourVitebskWebServiceApp.Helpers;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.Repositories
{
    public class VacanciesRepository : IVacancyRepository
    {
        private readonly YourVitebskDBContext _context;
        private readonly RolePermissionManager _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private bool _disposed = false;

        public VacanciesRepository(YourVitebskDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _roleManager = new RolePermissionManager(_context);
            _httpContextAccessor = httpContextAccessor;
        }

        public bool CheckRolePermission(string permission)
        {
            return _roleManager.HasPermission(_httpContextAccessor.HttpContext.User.Identity.Name, permission);
        }

        public IEnumerable<Vacancy> Get()
        {
            return _context.Vacancies.ToList().OrderBy(x => x.VacancyId);
        }

        public Vacancy Get(int id)
        {
            return _context.Vacancies.FirstOrDefault(x => x.VacancyId == id);
        }

        public void Create(Vacancy vacancy)
        {
            _context.Vacancies.Add(vacancy);
            _context.SaveChanges();
        }

        public void Update(Vacancy vacancy)
        {
            _context.Vacancies.Update(vacancy);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            _context.Vacancies.Remove(Get(id));
            _context.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
