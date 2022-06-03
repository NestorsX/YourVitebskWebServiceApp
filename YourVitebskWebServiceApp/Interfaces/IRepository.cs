using System;
using System.Collections.Generic;

namespace YourVitebskWebServiceApp.Interfaces
{
    public interface IRepository<T> : IDisposable
    {
        IEnumerable<IViewModel> Get();

        T Get(int id);

        void Create(T obj);

        void Update(T obj);

        void Delete(int id);
    }
}
