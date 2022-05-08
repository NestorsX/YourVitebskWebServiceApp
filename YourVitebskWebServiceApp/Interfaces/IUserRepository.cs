using System;
using System.Collections.Generic;
using YourVitebskWebServiceApp.Models;
using YourVitebskWebServiceApp.ViewModels;

namespace YourVitebskWebServiceApp.Interfaces
{
    public interface IUserRepository : IDisposable
    {
        IEnumerable<UserViewModel> Get();

        UserViewModel Get(int id);

        User GetUser(int id);

        void Create(User user);

        void Update(User user);

        void Delete(int id);
    }
}
