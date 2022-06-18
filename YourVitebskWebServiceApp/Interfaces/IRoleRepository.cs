using System;
using System.Collections.Generic;
using YourVitebskWebServiceApp.Models;
using YourVitebskWebServiceApp.ViewModels;

namespace YourVitebskWebServiceApp.Interfaces
{
    public interface IRoleRepository : IPermissionChecker, IDisposable
    {
        IEnumerable<RoleViewModel> Get();
        RoleViewModel GetForView(int id);
        RoleDTOViewModel GetForEdit(int id);
        void Create(RoleDTOViewModel role);
        void Update(RoleDTOViewModel role);
        void Delete(int id);
    }
}
