using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using YourVitebskWebServiceApp.APIServices;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;
using YourVitebskWebServiceApp.ViewModels;

namespace YourVitebskWebServiceApp.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly YourVitebskDBContext _context;
        private readonly IImageRepository<User> _repository;

        public UsersController(YourVitebskDBContext context, IImageRepository<User> repository)
        {
            _context = context;
            _repository = repository;
        }

        public ActionResult Index()
        {
            return View(_repository.Get());
        }

        public ActionResult Create()
        {
            ViewBag.Roles = _context.Roles;
            return View();
        }

        [HttpPost]
        public ActionResult Create(UserViewModel newUser, IFormFileCollection uploadedFiles)
        {
            if (_context.Users.FirstOrDefault(x => x.Email == newUser.Email) != null)
            {
                ModelState.AddModelError("Email", "Email уже используется");
            }

            if (!string.IsNullOrEmpty(newUser.PhoneNumber))
            {
                if (_context.Users.FirstOrDefault(x => x.PhoneNumber == newUser.PhoneNumber) != null)
                {
                    ModelState.AddModelError("PhoneNumber", "Такой номер телефона уже используется");
                }
            }

            if (ModelState.IsValid)
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

                _repository.Create(user, uploadedFiles);
                return RedirectToAction("Index");
            }

            ViewBag.Roles = _context.Roles;
            return View(newUser);
        }

        public ActionResult Edit(int id)
        {
            if (id == 1)
            {
                return RedirectToAction("Index");
            }

            UserViewModel user = (UserViewModel)_repository.Get(id);
            if (user != null)
            {
                user.Password = null;
                ViewBag.Roles = _context.Roles;
                return View(user);
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult Edit(UserViewModel newUser, IFormFileCollection uploadedFiles)
        {
            User user = _context.Users.FirstOrDefault(x => x.UserId == newUser.UserId);
            if (_context.Users.FirstOrDefault(x => x.Email == newUser.Email && newUser.Email != user.Email) != null)
            {
                ModelState.AddModelError("Email", "Email уже используется");
            }

            if (!string.IsNullOrWhiteSpace(newUser.PhoneNumber))
            {
                if (_context.Users.FirstOrDefault(x => x.PhoneNumber == newUser.PhoneNumber && newUser.PhoneNumber != user.PhoneNumber) != null)
                {
                    ModelState.AddModelError("UserDatum.PhoneNumber", "Такой номер телефона уже используется");
                }
            }

            if (ModelState.IsValid)
            {
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
                _repository.Update(user, uploadedFiles);
                return RedirectToAction("Index");
            }

            ViewBag.Roles = _context.Roles;
            return View(newUser);
        }

        [HttpGet]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            if (id == 1)
            {
                return RedirectToAction("Index");
            }

            UserViewModel user = (UserViewModel)_repository.Get(id);
            if (user != null)
            {
                return View(user);
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            _repository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
