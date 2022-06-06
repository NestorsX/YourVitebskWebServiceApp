using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using YourVitebskWebServiceApp.APIServiceInterfaces;
using YourVitebskWebServiceApp.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using YourVitebskWebServiceApp.APIModels;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using YourVitebskWebServiceApp.Repositories;
using Microsoft.AspNetCore.Http;

namespace YourVitebskWebServiceApp.APIServices
{
    public class AuthService : IAuthService
    {
        private readonly YourVitebskDBContext _context;
        private readonly IConfiguration _configuration;
        private readonly ImageService _imageService;
        private readonly IWebHostEnvironment _appEnvironment;

        public AuthService(YourVitebskDBContext context, IConfiguration configuration, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _configuration = configuration;
            _appEnvironment = appEnvironment;
            _imageService = new ImageService(appEnvironment);
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
            string image = "";
            if (Directory.Exists($"{_appEnvironment.WebRootPath}/images/users/{user.UserId}"))
            {
                image = Directory.GetFiles($"{_appEnvironment.WebRootPath}/images/users/{user.UserId}").Select(x => Path.GetFileName(x)).First();
            }

            var claims = new List<Claim>()
            {
                new Claim(nameof(user.UserId), user.UserId.ToString()),
                new Claim(nameof(user.Email), user.Email),
                new Claim(nameof(user.UserDatum.FirstName), user.UserDatum.FirstName),
                new Claim(nameof(user.UserDatum.LastName), user.UserDatum.LastName),
                new Claim(nameof(user.UserDatum.PhoneNumber), user.UserDatum.PhoneNumber),
                new Claim(nameof(user.IsVisible), user.IsVisible.ToString()),
                new Claim("Image", image)
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

                    CreatePasswordHash(userData.Password, out byte[] passwordHash, out byte[] passwordSalt);
                    var user = new Models.User
                    {
                        UserId = null,
                        Email = userData.Email,
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                        RoleId = 1,
                        IsVisible = true,
                        UserDatum = null
                    };

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                    user.UserDatum = new UserDatum
                    {
                        UserDataId = null,
                        UserId = user.UserId,
                        FirstName = userData.FirstName,
                        LastName = userData.LastName,
                        PhoneNumber = null
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

        public async Task<string> Update(APIModels.User newUser)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    Models.User user = await _context.Users.Include(x => x.UserDatum).FirstOrDefaultAsync(x => x.UserId == newUser.UserId);
                    if (await _context.Users.AnyAsync(x => x.Email == newUser.Email && x.UserId != newUser.UserId))
                    {
                        throw new ArgumentException("Пользователь с таким email уже существует!");
                    }

                    if (newUser.PhoneNumber != null && await _context.UserData.AnyAsync(x => x.PhoneNumber == newUser.PhoneNumber && x.UserId != newUser.UserId))
                    {
                        throw new ArgumentException("Этот номер телефона уже привязан к другому аккаунту");
                    }

                    if (!string.IsNullOrEmpty(newUser.OldPassword) && !string.IsNullOrEmpty(newUser.NewPassword))
                    {
                        if (VerifyPassword(newUser.OldPassword, user.PasswordHash, user.PasswordSalt))
                        {
                            CreatePasswordHash(newUser.NewPassword, out byte[] hash, out byte[] salt);
                            user.PasswordHash = hash;
                            user.PasswordSalt = salt;
                        }
                    }

                    if (newUser.Image != null)
                    {
                        _imageService.SaveImages("users", (int)user.UserId, new FormFileCollection
                        {
                            new FormFile(new MemoryStream(newUser.Image), 0, newUser.Image.Length, "image", "avatar.jpg"),
                        });
                    }

                    user.Email = newUser.Email;
                    user.UserDatum.FirstName = newUser.FirstName;
                    user.UserDatum.LastName = newUser.LastName;
                    user.UserDatum.PhoneNumber = newUser.PhoneNumber;
                    _context.Entry(user).State = EntityState.Modified;
                    _context.Entry(user.UserDatum).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return CreateToken(user);
                }
                catch (ArgumentException ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
    }
}
