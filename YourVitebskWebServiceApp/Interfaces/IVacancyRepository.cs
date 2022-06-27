using System;
using System.Collections.Generic;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.Interfaces
{
    public interface IVacancyRepository : IPermissionChecker, IDisposable
    {
        IEnumerable<Vacancy> Get();

        Vacancy Get(int id);

        void Create(Vacancy obj);

        void Update(Vacancy obj);

        void Delete(int id);
    }
}
