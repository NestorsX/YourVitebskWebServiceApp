using System.Collections.Generic;
using System.Threading.Tasks;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.APIServiceInterfaces
{
    public interface IUsersService
    {
        public Task<IEnumerable<User>> GetAllUsers();
        public Task<User> GetById(int id);
        public Task Update(User newUser);
    }
}
