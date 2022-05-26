using System.Threading.Tasks;
using YourVitebskWebServiceApp.APIModels;

namespace YourVitebskWebServiceApp.APIServiceInterfaces
{
    public interface IAuthService
    {
        public Task<string> Register(UserRegisterDTO userData);
        public Task<string> Login(UserLoginDTO userData);
        public Task<string> Update(User userData);
    }
}
