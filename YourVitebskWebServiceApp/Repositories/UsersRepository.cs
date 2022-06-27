using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using YourVitebskWebServiceApp.APIServices;
using YourVitebskWebServiceApp.Helpers;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;
using YourVitebskWebServiceApp.ViewModels;

namespace YourVitebskWebServiceApp.Repositories
{
    public class UsersRepository : IUserRepository
    {
        private readonly YourVitebskDBContext _context;
        private readonly ImageService _imageService;
        private readonly RolePermissionManager _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private bool _disposed = false;

        public UsersRepository(YourVitebskDBContext context, IWebHostEnvironment appEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _imageService = new ImageService(appEnvironment);
            _roleManager = new RolePermissionManager(_context);
            _httpContextAccessor = httpContextAccessor;
        }

        public bool CheckRolePermission(string permission)
        {
            return _roleManager.HasPermission(_httpContextAccessor.HttpContext.User.Identity.Name, permission);
        }

        public IEnumerable<UserViewModel> Get()
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

        public UserViewModel Get(int id)
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

        public void Create(UserViewModel newUser, IFormFileCollection uploadedFiles)
        {
            AuthService.CreatePasswordHash(newUser.Password, out byte[] hash, out byte[] salt);
            var user = new User
            {
                UserId = null,
                Email = newUser.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                IsVisible = newUser.IsVisible,
                RoleId = newUser.RoleId,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                PhoneNumber = newUser.PhoneNumber ?? "",

            };
            _context.Users.Add(user);
            _context.SaveChanges();
            _imageService.SaveImages("users", (int)user.UserId, uploadedFiles);
        }

        public void Update(UserViewModel newUser, IFormFileCollection uploadedFiles)
        {
            var user = _context.Users.First(x => x.UserId == newUser.UserId);
            if (!string.IsNullOrEmpty(newUser.Password))
            {
                AuthService.CreatePasswordHash(newUser.Password, out byte[] hash, out byte[] salt);
                user.PasswordHash = hash;
                user.PasswordSalt = salt;
            }

            user.Email = newUser.Email;
            user.RoleId = newUser.RoleId;
            user.IsVisible = newUser.IsVisible;
            user.FirstName = newUser.FirstName;
            user.LastName = newUser.LastName;
            user.PhoneNumber = newUser.PhoneNumber ?? "";
            _context.Users.Update(user);
            _context.SaveChanges();
            _imageService.SaveImages("users", (int)user.UserId, uploadedFiles);
        }

        public void Delete(int id)
        {
            User user = _context.Users.FirstOrDefault(x => x.UserId == id);
            _context.Users.Remove(user);
            _context.SaveChanges();
            _imageService.DeleteImages("users", id);
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
