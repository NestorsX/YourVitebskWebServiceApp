using System;
using System.Collections.Generic;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.Interfaces
{
    public interface IPosterTypeRepository : IPermissionChecker, IDisposable
    {
        IEnumerable<PosterType> Get();

        PosterType Get(int id);

        void Create(PosterType obj);

        void Update(PosterType obj);

        void Delete(int id);
    }
}
