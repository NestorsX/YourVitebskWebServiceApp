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
using System.Text;
using Microsoft.Extensions.Configuration;
using YourVitebskWebServiceApp.APIModels;

namespace YourVitebskWebServiceApp.APIServices
{
    public class AuthService : IAuthService
    {
        private readonly YourVitebskDBContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(YourVitebskDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public static bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        public string CreateToken(Models.User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(nameof(user.UserId), user.UserId.ToString()),
                new Claim(nameof(user.Email), user.Email),
                new Claim(nameof(user.UserDatum.FirstName), user.UserDatum.FirstName),
                new Claim(nameof(user.UserDatum.SecondName), user.UserDatum.SecondName),
                new Claim(nameof(user.UserDatum.LastName), user.UserDatum.LastName),
                new Claim(nameof(user.UserDatum.PhoneNumber), user.UserDatum.PhoneNumber)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> Register(UserRegisterDTO userData)
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
                    var user = new Models.User
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

        public async Task<string> Login(UserLoginDTO userData)
        {
            Models.User user = await _context.Users.Include(x => x.UserDatum).FirstOrDefaultAsync(x => x.Email == userData.Email);
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
