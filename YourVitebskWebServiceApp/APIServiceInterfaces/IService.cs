using System.Collections.Generic;
using System.Threading.Tasks;

namespace YourVitebskWebServiceApp.APIServiceInterfaces
{
    public interface IService<T>
    {
        public Task<IEnumerable<T>> GetAll();
        public Task<T> GetById(int id);
    }
}
