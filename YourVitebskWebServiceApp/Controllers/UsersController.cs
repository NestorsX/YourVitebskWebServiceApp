using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly YourVitebskDBContext _context;
        private readonly IRepository<User> _repository;

        public UsersController(YourVitebskDBContext context, IRepository<User> repository)
        {
            _context = context;
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View(_repository.Get());
        }

        public ActionResult Create()
        {
            ViewBag.Roles = _context.Roles;
            return View();
        }

        [HttpPost]
        public IActionResult CreateAsync(User newUser)
        {
            ViewBag.Roles = _context.Roles;
            if (_context.Users.FirstOrDefault(x => x.Email == newUser.Email) != null)
            {
                ModelState.AddModelError("Email", "Email уже используется");
            }

            if (newUser.RoleId == 0)
            {
                ModelState.AddModelError("RoleId", "Выберите роль");
            }

            if (newUser.UserDatum.PhoneNumber != null)
            {
                if (_context.Users.FirstOrDefault(x => x.UserDatum.PhoneNumber == newUser.UserDatum.PhoneNumber) != null)
                {
                    ModelState.AddModelError("UserDatum.PhoneNumber", "Такой номер телефона уже используется");
                }
            }

            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserId = null,
                    Email = newUser.Email,
                    Password = newUser.Password,
                    RoleId = newUser.RoleId,
                    UserDatum = new UserDatum
                    {
                        UserDataId = null,
                        UserId = null,
                        FirstName = newUser.UserDatum.FirstName,
                        SecondName = newUser.UserDatum.SecondName,
                        LastName = newUser.UserDatum.LastName,
                        PhoneNumber = newUser.UserDatum.PhoneNumber,
                    }
                };

                _repository.Create(user);
                return RedirectToAction("Index");
            }

            return View(newUser);
        }

        public ActionResult Edit(int id)
        {
            if (id == 1)
            {
                return RedirectToAction("Index");
            }

            User user = _repository.Get(id);
            if (user != null)
            {
                ViewBag.Roles = _context.Roles;
                ViewData["RoleId"] = user.RoleId;
                return View(user);
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult Edit(User newUser)
        {
            ViewBag.Roles = _context.Roles;
            User user = _repository.Get((int)newUser.UserId);
            if (_context.Users.FirstOrDefault(x => x.Email == newUser.Email && newUser.Email != user.Email) != null)
            {
                ModelState.AddModelError("Email", "Email уже используется");
            }

            if (newUser.RoleId == 0)
            {
                ViewData["RoleId"] = 0;
                ModelState.AddModelError("RoleId", "Выберите роль");
            }

            if (newUser.UserDatum.PhoneNumber != null)
            {
                if (_context.Users.FirstOrDefault(x => x.UserDatum.PhoneNumber == newUser.UserDatum.PhoneNumber && newUser.UserDatum.PhoneNumber != user.UserDatum.PhoneNumber) != null)
                {
                    ModelState.AddModelError("UserDatum.PhoneNumber", "Такой номер телефона уже используется");
                }
            }

            if (ModelState.IsValid)
            {
                user.Email = newUser.Email;
                user.Password = newUser.Password;
                user.RoleId = newUser.RoleId;
                user.UserDatum.FirstName = newUser.UserDatum.FirstName;
                user.UserDatum.SecondName = newUser.UserDatum.SecondName;
                user.UserDatum.LastName = newUser.UserDatum.LastName;
                user.UserDatum.PhoneNumber = newUser.UserDatum.PhoneNumber;
                _repository.Update(user);
                return RedirectToAction("Index");
            }

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

            User user = _repository.Get(id);
            if (user != null)
            {
                ViewData["RoleName"] = _context.Roles.First(x => x.RoleId == user.RoleId).Name;
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
