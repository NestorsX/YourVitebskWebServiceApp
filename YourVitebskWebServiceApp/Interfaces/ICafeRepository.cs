using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using YourVitebskWebServiceApp.Models;
using YourVitebskWebServiceApp.ViewModels;

namespace YourVitebskWebServiceApp.Interfaces
{
    public interface ICafeRepository : IPermissionChecker, IDisposable
    {
        IEnumerable<CafeViewModel> Get();

        Cafe Get(int id);

        void Create(Cafe obj, IFormFileCollection uploadedFiles);

        void Update(Cafe obj, IFormFileCollection uploadedFiles);

        void Delete(int id);
    }
}
