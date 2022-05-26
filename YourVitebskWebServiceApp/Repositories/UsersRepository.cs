using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;
using YourVitebskWebServiceApp.ViewModels;

namespace YourVitebskWebServiceApp.Repositories
{
    public class UsersRepository : IUserRepository
    {
        private readonly YourVitebskDBContext _context;
        private bool _disposed = false;

        public UsersRepository(YourVitebskDBContext context)
        {
            _context = context;
        }

        public IEnumerable<UserViewModel> Get()
        {
            IEnumerable<UserViewModel> result = new List<UserViewModel>();
            IEnumerable<User> users = _context.Users.Include(x => x.UserDatum).ToList().OrderBy(x => x.UserId);
            foreach (User user in users)
            {
                result = result.Append(new UserViewModel()
                {
                    UserId = (int)user.UserId,
                    Email = user.Email,
                    RoleId = user.RoleId,
                    Role = _context.Roles.First(x => x.RoleId == user.RoleId).Name,
                    IsVisible = user.IsVisible,
                    FirstName = user.UserDatum.FirstName,
                    LastName = user.UserDatum.LastName,
                    PhoneNumber = user.UserDatum.PhoneNumber
                });
            }

            return result;
        }

        public UserViewModel Get(int id)
        {
            User user = GetUser(id);
            var result = new UserViewModel()
            {
                UserId = (int)user.UserId,
                Email = user.Email,
                RoleId = user.RoleId,
                Role = _context.Roles.First(x => x.RoleId == user.RoleId).Name,
                IsVisible = user.IsVisible,
                FirstName = user.UserDatum.FirstName,
                LastName = user.UserDatum.LastName,
                PhoneNumber = user.UserDatum.PhoneNumber
            };

            return result;
        }

        public User GetUser(int id)
        {
            return _context.Users.Include(x => x.UserDatum).FirstOrDefault(x => x.UserId == id);
        }

        public void Create(User user)
        {
            UserDatum userData = user.UserDatum;
            user.UserDatum = null;
            _context.Users.Add(user);
            _context.SaveChanges();
            userData.UserId = user.UserId;
            _context.UserData.Add(userData);
            _context.SaveChanges();
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            User user = GetUser(id);
            if (user.UserDatum != null)
            {
                _context.UserData.Remove(user.UserDatum);
            }

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
