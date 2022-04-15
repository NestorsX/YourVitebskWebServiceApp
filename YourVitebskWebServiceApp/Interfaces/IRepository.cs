using System.Collections.Generic;

namespace YourVitebskWebServiceApp.Interfaces
{
    public interface IRepository<T>
    {
        IEnumerable<T> Get();

        T Get(int id);

        void Create(T user);

        void Update(T user);

        void Delete(int id);
    }
}
