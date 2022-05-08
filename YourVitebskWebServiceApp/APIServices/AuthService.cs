using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YourVitebskWebServiceApp.APIServiceInterfaces;
using YourVitebskWebServiceApp.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace YourVitebskWebServiceApp.APIServices
{
    public class AuthService : IAuthService
    {
        private readonly YourVitebskDBContext _context;

        public AuthService(YourVitebskDBContext context)
        {
            _context = context;
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password), "Password argument cannot be null or empty");
            }

            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(nameof(user.UserId), user.UserId.ToString()),
                new Claim(nameof(user.Email), user.Email),
                new Claim(nameof(user.RoleId), user.RoleId.ToString()),
                new Claim(nameof(user.UserDatum.FirstName), user.UserDatum.FirstName),
                new Claim(nameof(user.UserDatum.SecondName), user.UserDatum.SecondName),
                new Claim(nameof(user.UserDatum.LastName), user.UserDatum.LastName),
                new Claim(nameof(user.UserDatum.PhoneNumber), user.UserDatum.PhoneNumber)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("this is my custom Secret key for authentication"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> Register(UserDTO userData)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (await _context.Users.AnyAsync(x => x.Email == userData.Email))
                    {
                        throw new ArgumentException("Пользователь с таким email уже существует!");
                    }

                    if (!string.IsNullOrEmpty(userData.PhoneNumber))
                    {
                        if (await _context.UserData.AnyAsync(x => x.PhoneNumber == userData.PhoneNumber))
                        {
                            throw new ArgumentException("Этот номер телефона уже привязан к другому аккаунту");
                        }

                        if (!Regex.IsMatch(userData.PhoneNumber, @"^\+375\((33|29|44|25)\)(\d{3})\-(\d{2})\-(\d{2})"))
                        {
                            throw new ArgumentException("Введенный номер не соответсвует формату");
                        }
                    }

                    CreatePasswordHash(userData.Password, out byte[] passwordHash, out byte[] passwordSalt);
                    var user = new User
                    {
                        UserId = null,
                        Email = userData.Email,
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                        RoleId = 1,
                        UserDatum = null
                    };

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                    user.UserDatum = new UserDatum
                    {
                        UserDataId = null,
                        UserId = user.UserId,
                        FirstName = userData.FirstName,
                        SecondName = userData.SecondName,
                        LastName = userData.LastName,
                        PhoneNumber = userData.PhoneNumber
                    };

                    _context.UserData.Add(user.UserDatum);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return CreateToken(user);
                }
                catch (ArgumentException e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }
        }

        public async Task<string> Login(UserDTO userData)
        {
            User user = await _context.Users.Include(x => x.UserDatum).FirstOrDefaultAsync(x => x.Email == userData.Email);
            if (user == null)
            {
                throw new ArgumentException("Неверные логин и(или) пароль");
            }

            if (!VerifyPassword(userData.Password, user.PasswordHash, user.PasswordSalt))
            {
                throw new ArgumentException("Неверные логин и(или) пароль");
            }

            return CreateToken(user);
        }
    }
}
