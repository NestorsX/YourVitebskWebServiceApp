using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        private readonly YourVitebskDBContext _context;
        private readonly IRepository<Role> _repository;

        public RolesController(YourVitebskDBContext context, IRepository<Role> repository)
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
            return View();
        }

        [HttpPost]
        public IActionResult CreateAsync(Role newRole)
        {
            if (_context.Roles.FirstOrDefault(x => x.Name == newRole.Name) != null)
            {
                ModelState.AddModelError("Name", "Такая роль уже существует");
            }

            if (ModelState.IsValid)
            {
                var role = new Role
                {
                    RoleId = null,
                    Name = newRole.Name
                };

                _repository.Create(role);
                return RedirectToAction("Index");
            }

            return View(newRole);
        }

        public ActionResult Edit(int id)
        {
            if (id == 1 || id == 2)
            {
                return RedirectToAction("Index");
            }

            Role role = _repository.Get(id);
            if (role != null)
                return View(role);
            return NotFound();
        }

        [HttpPost]
        public ActionResult Edit(Role newRole)
        {
            Role role = _repository.Get((int)newRole.RoleId);
            if (_context.Roles.FirstOrDefault(x => x.Name == newRole.Name && role.Name != newRole.Name) != null)
            {
                ModelState.AddModelError("Name", "Такая роль уже существует");
            }

            if (ModelState.IsValid)
            {
                role.Name = newRole.Name;
                _repository.Update(role);
                return RedirectToAction("Index");
            }

            return View(newRole);
        }

        [HttpGet]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            if (id == 1 || id == 2)
            {
                return RedirectToAction("Index");
            }

            Role role = _repository.Get(id);
            if (role != null)
            {
                return View(role);
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
