using System.Collections.Generic;
using System.Threading.Tasks;
using YourVitebskWebServiceApp.APIModels;

namespace YourVitebskWebServiceApp.APIServiceInterfaces
{
    public interface IUsersService
    {
        public Task<IEnumerable<UsersListItem>> GetAllUsers(int id);
    }
}
