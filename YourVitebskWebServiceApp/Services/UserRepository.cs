using System.Collections.Generic;
using System.Linq;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.Services
{
    public class UserRepository : IRepository<User>
    {
        private readonly YourVitebskDBContext _context;

        public UserRepository(YourVitebskDBContext context)
        {
            _context = context;
        }

        private UserDatum GetUserData(int userId)
        {
            return _context.UserData.FirstOrDefault(x => x.UserId == userId);
        }

        public IEnumerable<User> Get()
        {
            IEnumerable<User> users = _context.Users.ToList().OrderBy(x => x.UserId);
            foreach (User user in users)
            {
                user.UserDatum = GetUserData((int)user.UserId);
            }
            
            return users;
        }

        public User Get(int id)
        {
            User user = _context.Users.FirstOrDefault(x => x.UserId == id);
            user.UserDatum = GetUserData((int)user.UserId);
            return user;
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
            User user = Get(id);
            if (user.UserDatum != null)
            {
                _context.UserData.Remove(user.UserDatum);
            }

            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
}
