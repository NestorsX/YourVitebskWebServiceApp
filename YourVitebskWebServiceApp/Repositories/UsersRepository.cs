using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;
using YourVitebskWebServiceApp.ViewModels;

namespace YourVitebskWebServiceApp.Repositories
{
    public class UsersRepository : IImageRepository<User>
    {
        private readonly YourVitebskDBContext _context;
        private readonly ImageService _imageService;
        private bool _disposed = false;

        public UsersRepository(YourVitebskDBContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _imageService = new ImageService(appEnvironment);
        }

        public IEnumerable<IViewModel> Get()
        {
            IEnumerable<UserViewModel> result = new List<UserViewModel>();
            IEnumerable<User> users = _context.Users.ToList().OrderBy(x => x.UserId);
            foreach (User user in users)
            {
                result = result.Append(new UserViewModel()
                {
                    UserId = (int)user.UserId,
                    Email = user.Email,
                    RoleId = user.RoleId,
                    Role = _context.Roles.First(x => x.RoleId == user.RoleId).Name,
                    IsVisible = user.IsVisible,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber
                });
            }

            return result;
        }

        public IViewModel Get(int id)
        {
            User user = _context.Users.FirstOrDefault(x => x.UserId == id);
            var result = new UserViewModel()
            {
                UserId = (int)user.UserId,
                Email = user.Email,
                RoleId = user.RoleId,
                Role = _context.Roles.First(x => x.RoleId == user.RoleId).Name,
                IsVisible = user.IsVisible,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber
            };

            return result;
        }

        public void Create(User user, IFormFileCollection uploadedFiles)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            _imageService.SaveImages("users", (int)user.UserId, uploadedFiles);
        }

        public void Update(User user, IFormFileCollection uploadedFiles)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
            _imageService.SaveImages("users", (int)user.UserId, uploadedFiles);
        }

        public void Delete(int id)
        {
            User user = _context.Users.FirstOrDefault(x => x.UserId == id);
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
