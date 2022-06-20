using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using YourVitebskWebServiceApp.Helpers.Filterers;
using YourVitebskWebServiceApp.Helpers.Sorters;
using YourVitebskWebServiceApp.Helpers.SortStates;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;
using YourVitebskWebServiceApp.ViewModels;
using YourVitebskWebServiceApp.ViewModels.IndexViewModels;

namespace YourVitebskWebServiceApp.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly YourVitebskDBContext _context;
        private readonly IImageRepository<UserViewModel> _repository;
        private readonly AccountController _account;

        public UsersController(YourVitebskDBContext context, IImageRepository<UserViewModel> repository)
        {
            _context = context;
            _repository = repository;
            _account = new AccountController(context);
        }

        public ActionResult Index(int? role, string search, UserSortStates sort = UserSortStates.UserIdAsc, int page = 1)
        {
            try
            {
                if (!_repository.CheckRolePermission(HttpContext.User.Identity.Name, nameof(Helpers.RolePermission.UsersGet)))
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }
            catch (ArgumentException)
            {
                return RedirectToAction("Logout", "Account");
            }

            var users = (IEnumerable<UserViewModel>)_repository.Get();

            if (role != null && role != 0)
            {
                users = users.Where(x => x.RoleId == role);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                users = users.Where(x => x.UserId.ToString().Contains(search) ||
                                         x.Email.ToLower().Contains(search.ToLower()) ||
                                         x.FirstName.ToLower().Contains(search.ToLower()) ||
                                         x.LastName.ToLower().Contains(search.ToLower()) ||
                                         x.PhoneNumber.Contains(search)
                );
            }
            
            users = sort switch
            {
                UserSortStates.UserIdDesc => users.OrderByDescending(x => x.UserId),
                UserSortStates.RoleAsc => users.OrderBy(x => x.Role),
                UserSortStates.RoleDesc => users.OrderByDescending(x => x.Role),
                UserSortStates.EmailAsc => users.OrderBy(x => x.Email),
                UserSortStates.EmailDesc => users.OrderByDescending(x => x.Email),
                UserSortStates.FirstNameAsc => users.OrderBy(x => x.FirstName),
                UserSortStates.FirstNameDesc => users.OrderByDescending(x => x.FirstName),
                UserSortStates.LastNameAsc => users.OrderBy(x => x.LastName),
                UserSortStates.LastNameDesc => users.OrderByDescending(x => x.LastName),
                UserSortStates.PhoneNumberAsc => users.OrderBy(x => x.PhoneNumber),
                UserSortStates.PhoneNumberDesc => users.OrderByDescending(x => x.PhoneNumber),
                UserSortStates.VisibleAsc => users.OrderBy(x => x.IsVisible),
                UserSortStates.VisibleDesc => users.OrderByDescending(x => x.IsVisible),
                _ => users.OrderBy(x => x.UserId),
            };

            const int pageSize = 5;
            if (page < 1)
            {
                page = 1;
            }

            int count = users.Count();
            var pager = new Pager(count, page, pageSize);
            int skip = (page - 1) * pageSize;
            users = users.Skip(skip).Take(pager.PageSize);

            var viewModel = new UserIndexViewModel()
            {
                Pager = pager,
                Sorter = new UserSorter(sort),
                Filterer = new UserFilterer(_context.Roles.ToList(), role, search),
                Data = users.ToList()
            };

            return View(viewModel);
        }

        public ActionResult Create()
        {
            try
            {
                if (!_repository.CheckRolePermission(HttpContext.User.Identity.Name, nameof(Helpers.RolePermission.UsersCreate)))
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }
            catch (ArgumentException)
            {
                return RedirectToAction("Logout", "Account");
            }

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
                _repository.Create(newUser, uploadedFiles);
                return RedirectToAction("Index");
            }

            ViewBag.Roles = _context.Roles;
            return View(newUser);
        }

        public ActionResult Edit(int id)
        {
            try
            {
                if (!_repository.CheckRolePermission(HttpContext.User.Identity.Name, nameof(Helpers.RolePermission.UsersUpdate)))
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }
            catch (ArgumentException)
            {
                return RedirectToAction("Logout", "Account");
            }

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
            UserViewModel currentUser = (UserViewModel)_repository.Get(newUser.UserId);
            if (_context.Users.FirstOrDefault(x => x.Email == newUser.Email && newUser.Email != currentUser.Email) != null)
            {
                ModelState.AddModelError("Email", "Email уже используется");
            }

            if (!string.IsNullOrWhiteSpace(newUser.PhoneNumber))
            {
                if (_context.Users.FirstOrDefault(x => x.PhoneNumber == newUser.PhoneNumber && newUser.PhoneNumber != currentUser.PhoneNumber) != null)
                {
                    ModelState.AddModelError("UserDatum.PhoneNumber", "Такой номер телефона уже используется");
                }
            }

            if (ModelState.IsValid)
            {
                _repository.Update(newUser, uploadedFiles);
                return RedirectToAction("Index");
            }

            ViewBag.Roles = _context.Roles;
            return View(newUser);
        }

        [HttpGet]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            try
            {
                if (!_repository.CheckRolePermission(HttpContext.User.Identity.Name, nameof(Helpers.RolePermission.UsersDelete)))
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }
            catch (ArgumentException)
            {
                return RedirectToAction("Logout", "Account");
            }

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
