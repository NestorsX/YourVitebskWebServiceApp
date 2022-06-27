using System;
using System.Collections.Generic;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.Interfaces
{
    public interface ICafeTypeRepository : IPermissionChecker, IDisposable
    {
        IEnumerable<CafeType> Get();

        CafeType Get(int id);

        void Create(CafeType obj);

        void Update(CafeType obj);

        void Delete(int id);
    }
}
