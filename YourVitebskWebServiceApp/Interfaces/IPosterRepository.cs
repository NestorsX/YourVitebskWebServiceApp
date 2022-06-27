using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using YourVitebskWebServiceApp.Models;
using YourVitebskWebServiceApp.ViewModels;

namespace YourVitebskWebServiceApp.Interfaces
{
    public interface IPosterRepository : IPermissionChecker, IDisposable
    {
        IEnumerable<PosterViewModel> Get();

        Poster Get(int id);

        void Create(Poster obj, IFormFileCollection uploadedFiles);

        void Update(Poster obj, IFormFileCollection uploadedFiles);

        void Delete(int id);
    }
}
