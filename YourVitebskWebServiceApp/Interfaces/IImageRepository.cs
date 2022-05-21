using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace YourVitebskWebServiceApp.Interfaces
{
    public interface IImageRepository<T> : IDisposable
    {
        IEnumerable<IViewModel> Get();

        T Get(int id);

        void Create(T user, IFormFileCollection uploadedFiles);

        void Update(T user, IFormFileCollection uploadedFiles);

        void Delete(int id);
    }
}
