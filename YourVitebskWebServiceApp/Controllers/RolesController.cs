using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class RolesController : Controller
    {
        private readonly YourVitebskDBContext _context;
        private readonly IRoleRepository _repository;

        public RolesController(YourVitebskDBContext context, IRoleRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        public ActionResult Index(string search, RoleSortStates sort = RoleSortStates.RoleIdAsc, int page = 1)
        {
            var roles = _repository.Get();
            if (!string.IsNullOrWhiteSpace(search))
            {
                roles = roles.Where(x => x.RoleId.ToString().Contains(search) ||
                                         x.Name.ToLower().Contains(search.ToLower()) || 
                                         x.Permissions.Any(x => x.ToLower().Contains(search.ToLower())));
            }

            roles = sort switch
            {
                RoleSortStates.RoleIdDesc => roles.OrderByDescending(x => x.RoleId),
                RoleSortStates.NameAsc => roles.OrderBy(x => x.Name),
                RoleSortStates.NameDesc => roles.OrderByDescending(x => x.Name),
                _ => roles.OrderBy(x => x.RoleId),
            };

            const int pageSize = 5;
            if (page < 1)
            {
                page = 1;
            }

            int count = roles.Count();
            var pager = new Pager(count, page, pageSize);
            int skip = (page - 1) * pageSize;
            roles = roles.Skip(skip).Take(pager.PageSize);

            var viewModel = new RoleIndexViewModel()
            {
                Pager = pager,
                Sorter = new RoleSorter(sort),
                Filterer = new RoleFilterer(search),
                Data = roles.ToList()
            };

            return View(viewModel);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(RoleDTOViewModel newRole)
        {
            if (_context.Roles.FirstOrDefault(x => x.Name == newRole.Name) != null)
            {
                ModelState.AddModelError("Name", "Такая роль уже существует");
            }

            if (ModelState.IsValid)
            {
                _repository.Create(newRole);
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

            RoleDTOViewModel role = _repository.GetForEdit(id);
            if (role != null)
            {
                return View(role);
            }

            return RedirectToAction("NotFound", "Home");
        }

        [HttpPost]
        public ActionResult Edit(RoleDTOViewModel newRole)
        {
            Role role = _context.Roles.First(x => x.RoleId == newRole.RoleId);
            if (_context.Roles.FirstOrDefault(x => x.Name == newRole.Name && role.Name != newRole.Name) != null)
            {
                ModelState.AddModelError("Name", "Такая роль уже существует");
            }

            if (ModelState.IsValid)
            {
                _repository.Update(newRole);
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

            RoleViewModel role = _repository.GetForView(id);
            if (role != null)
            {
                return View(role);
            }

            return RedirectToAction("NotFound", "Home");
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            _repository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
