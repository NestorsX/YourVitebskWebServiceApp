using System.Collections.Generic;
using System.Threading.Tasks;
using YourVitebskWebServiceApp.Models;
using YourVitebskWebServiceApp.ViewModels;

namespace YourVitebskWebServiceApp.APIServiceInterfaces
{
    public interface IAuthService
    {
        public Task<string> Register(UserDTO userData);
        public Task<string> Login(UserDTO userData);
    }
}
