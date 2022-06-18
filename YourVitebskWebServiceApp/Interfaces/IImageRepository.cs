using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace YourVitebskWebServiceApp.Interfaces
{
    public interface IImageRepository<T> : IPermissionChecker, IDisposable
    {
        IEnumerable<IViewModel> Get();

        IViewModel Get(int id);

        void Create(T obj, IFormFileCollection uploadedFiles);

        void Update(T obj, IFormFileCollection uploadedFiles);

        void Delete(int id);
    }
}
