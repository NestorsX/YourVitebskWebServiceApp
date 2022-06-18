using System;
using System.Collections.Generic;
using System.Linq;
using YourVitebskWebServiceApp.Helpers;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.Repositories
{
    public class VacanciesRepository : IRepository<Vacancy>
    {
        private readonly YourVitebskDBContext _context;
        private readonly RolePermissionManager _roleManager;
        private bool _disposed = false;

        public VacanciesRepository(YourVitebskDBContext context)
        {
            _context = context;
            _roleManager = new RolePermissionManager(_context);
        }

        public bool CheckRolePermission(string userEmail, string permission)
        {
            return _roleManager.HasPermission(userEmail, permission);
        }

        public IEnumerable<IViewModel> Get()
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
