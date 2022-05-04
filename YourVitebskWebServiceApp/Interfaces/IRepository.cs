using System;
using System.Collections.Generic;

namespace YourVitebskWebServiceApp.Interfaces
{
    public interface IRepository<T> : IDisposable
    {
        IEnumerable<IViewModel> Get();

        T Get(int id);

        void Create(T user);

        void Update(T user);

        void Delete(int id);
    }
}
