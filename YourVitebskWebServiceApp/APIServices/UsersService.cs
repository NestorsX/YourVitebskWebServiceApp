using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourVitebskWebServiceApp.APIServiceInterfaces;

namespace YourVitebskWebServiceApp.APIServices
{
    public class UsersService : IUsersService
    {
        private readonly YourVitebskDBContext _context;

        public UsersService(YourVitebskDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<APIModels.User>> GetAllUsers()
        {
            IEnumerable<APIModels.User> result = new List<APIModels.User>();
            IEnumerable<Models.User> users = await _context.Users.Include(x => x.UserDatum).ToListAsync();
            foreach (var user in users)
            {
                result = result.Append(new APIModels.User()
                {
                    UserId = (int)user.UserId,
                    Email = user.Email,
                    Password = null,
                    FirstName = user.UserDatum.FirstName,
                    SecondName = user.UserDatum.SecondName,
                    LastName = user.UserDatum.LastName,
                    PhoneNumber = user.UserDatum.PhoneNumber
                });
            }

            return result;
        }

        public async Task<APIModels.User> GetById(int id)
        {
            Models.User user = await _context.Users.Include(x => x.UserDatum).FirstOrDefaultAsync(x => x.UserId == id);
            var result = new APIModels.User()
            {
                UserId = (int)user.UserId,
                Email = user.Email,
                Password = null,
                FirstName = user.UserDatum.FirstName,
                SecondName = user.UserDatum.SecondName,
                LastName = user.UserDatum.LastName,
                PhoneNumber = user.UserDatum.PhoneNumber
            };

            return result;
        }

        public async Task Update(APIModels.User newUser)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    Models.User user = await _context.Users.Include(x => x.UserDatum).FirstOrDefaultAsync(x => x.UserId == newUser.UserId);
                    if (await _context.Users.AnyAsync(x => x.Email == newUser.Email && x.UserId != newUser.UserId))
                    {
                        throw new InvalidOperationException("Пользователь с таким email уже существует!");
                    }

                    if (newUser.PhoneNumber != null && await _context.UserData.AnyAsync(x => x.PhoneNumber == newUser.PhoneNumber && x.UserId != newUser.UserId))
                    {
                        throw new InvalidOperationException("Этот номер телефона уже привязан к другому аккаунту");
                    }

                    if (!string.IsNullOrEmpty(newUser.Password))
                    {
                        AuthService.CreatePasswordHash(newUser.Password, out byte[] hash, out byte[] salt);
                        user.PasswordHash = hash;
                        user.PasswordSalt = salt;
                    }

                    user.Email = newUser.Email;
                    user.UserDatum.FirstName = newUser.FirstName;
                    user.UserDatum.SecondName = newUser.SecondName;
                    user.UserDatum.LastName = newUser.LastName;
                    user.UserDatum.PhoneNumber = newUser.PhoneNumber;
                    _context.Entry(user).State = EntityState.Modified;
                    _context.Entry(user.UserDatum).State = EntityState.Modified;
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
