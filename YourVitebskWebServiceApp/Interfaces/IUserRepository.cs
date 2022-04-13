using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers();

        User Get(int id);

        void Create(User user);

        void Update(User user);

        void Delete(int id);
    }
}
