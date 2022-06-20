using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.Interfaces
{
    public interface INewsRepository : IPermissionChecker, IDisposable
    {
        IEnumerable<News> Get();

        News Get(int id);

        void Create(News obj, IFormFileCollection uploadedFiles);

        void Update(News obj, IFormFileCollection uploadedFiles);

        void Delete(int id);
    }
}
