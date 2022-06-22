using System.Collections.Generic;
using System.Threading.Tasks;
using YourVitebskWebServiceApp.APIModels;

namespace YourVitebskWebServiceApp.APIServiceInterfaces
{
    public interface IUsersService
    {
        Task<IEnumerable<UsersListItem>> GetAllUsers(int id);
        Task<string> GetCommentsCount(int id);
    }
}
