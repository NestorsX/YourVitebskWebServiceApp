using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using YourVitebskWebServiceApp.APIServiceInterfaces;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.APIServices
{
    public class UsersService : IUsersService
    {
        private readonly YourVitebskDBContext _context;

        public UsersService(YourVitebskDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.Users.Include(x => x.UserDatum).ToListAsync();
        }

        public async Task<User> GetByData(string email, string password)
        {
            try
            {
                User user =  await _context.Users.Include(x => x.UserDatum).FirstOrDefaultAsync(x => x.Email == email && x.Password == password);
                if (user == null)
                {
                    throw new InvalidOperationException("Неверный логин и(или) пароль!");
                }

                return user;
            }
            catch (InvalidOperationException ex)
            {
                throw ex;
            }
        }

        public async Task<User> GetById(int id)
        {

            return await _context.Users.Include(x => x.UserDatum).FirstOrDefaultAsync(x => x.UserId == id);
        }

        public async Task<User> Create(User newUser)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (await _context.Users.AnyAsync(x => x.Email == newUser.Email && x.UserId != newUser.UserId))
                    {
                        throw new InvalidOperationException("Пользователь с таким email уже существует!");
                    }

                    if (newUser.UserDatum.PhoneNumber != null && await _context.UserData.AnyAsync(x => x.PhoneNumber == newUser.UserDatum.PhoneNumber))
                    {
                        throw new InvalidOperationException("Этот номер телефона уже привязан к другому аккаунту");
                    }

                    newUser.UserId = null;
                    newUser.RoleId = 1;
                    newUser.UserDatum.UserDataId = null;
                    _context.Users.Add(newUser);
                    await _context.SaveChangesAsync();
                    newUser.UserDatum.UserId = newUser.UserId;
                    _context.UserData.Add(newUser.UserDatum);
                    transaction.Commit();
                    return newUser;
                }
                catch (InvalidOperationException ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public async Task Update(User newUser)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (await _context.Users.AnyAsync(x => x.Email == newUser.Email && x.UserId != newUser.UserId))
                    {
                        throw new InvalidOperationException("Пользователь с таким email уже существует!");
                    }

                    if (newUser.UserDatum.PhoneNumber != null && await _context.UserData.AnyAsync(x => x.PhoneNumber == newUser.UserDatum.PhoneNumber && x.UserId != newUser.UserId))
                    {
                        throw new InvalidOperationException("Этот номер телефона уже привязан к другому аккаунту");
                    }

                    _context.Entry(newUser).State = EntityState.Modified;
                    _context.Entry(newUser.UserDatum).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (InvalidOperationException ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
    }
}
