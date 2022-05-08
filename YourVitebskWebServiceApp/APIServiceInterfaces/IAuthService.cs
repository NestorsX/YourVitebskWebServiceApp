using System.Collections.Generic;
using System.Threading.Tasks;
using YourVitebskWebServiceApp.Models;
using YourVitebskWebServiceApp.ViewModels;

namespace YourVitebskWebServiceApp.APIServiceInterfaces
{
    public interface IAuthService
    {
        public Task<string> Register(UserRegisterDTO userData);
        public Task<string> Login(UserLoginDTO userData);
    }
}
