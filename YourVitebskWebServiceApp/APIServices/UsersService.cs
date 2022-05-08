using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<User> GetById(int id)
        {

            return await _context.Users.Include(x => x.UserDatum).FirstOrDefaultAsync(x => x.UserId == id);
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
