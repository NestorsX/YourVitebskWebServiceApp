using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using YourVitebskWebServiceApp.ViewModels;

namespace YourVitebskWebServiceApp.Interfaces
{
    public interface IUserRepository : IPermissionChecker, IDisposable
    {
        IEnumerable<UserViewModel> Get();

        UserViewModel Get(int id);

        void Create(UserViewModel obj, IFormFileCollection uploadedFiles);

        void Update(UserViewModel obj, IFormFileCollection uploadedFiles);

        void Delete(int id);
    }
}
