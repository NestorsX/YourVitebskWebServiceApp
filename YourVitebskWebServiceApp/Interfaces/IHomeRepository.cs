using System;
using YourVitebskWebServiceApp.ViewModels.IndexViewModels;

namespace YourVitebskWebServiceApp.Interfaces
{
    public interface IHomeRepository : IDisposable
    {
        HomeIndexViewModel Get(string email);
    }
}
