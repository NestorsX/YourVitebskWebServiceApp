using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using YourVitebskWebServiceApp.Helpers.Filterers;
using YourVitebskWebServiceApp.Helpers.Sorters;
using YourVitebskWebServiceApp.Helpers.SortStates;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;
using YourVitebskWebServiceApp.ViewModels.IndexViewModels;

namespace YourVitebskWebServiceApp.Controllers
{
    [Authorize]
    public class CafeTypesController : Controller
    {
        private readonly ICafeTypeRepository _repository;

        public CafeTypesController(ICafeTypeRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Index(string search, CafeTypeSortStates sort = CafeTypeSortStates.CafeTypeIdAsc, int page = 1)
        {
            try
            {
                if (!_repository.CheckRolePermission(nameof(Helpers.RolePermission.CafesGet)))
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }
            catch (ArgumentException)
            {
                return RedirectToAction("Logout", "Account");
            }

            var cafeTypes = _repository.Get();
            if (!string.IsNullOrWhiteSpace(search))
            {
                cafeTypes = cafeTypes.Where(x => x.CafeTypeId.ToString().Contains(search) ||
                                         x.Name.ToLower().Contains(search.ToLower()));
            }

            cafeTypes = sort switch
            {
                CafeTypeSortStates.CafeTypeIdDesc => cafeTypes.OrderByDescending(x => x.CafeTypeId),
                CafeTypeSortStates.NameAsc => cafeTypes.OrderBy(x => x.Name),
                CafeTypeSortStates.NameDesc => cafeTypes.OrderByDescending(x => x.Name),
                _ => cafeTypes.OrderBy(x => x.CafeTypeId),
            };

            const int pageSize = 5;
            if (page < 1)
            {
                page = 1;
            }

            int count = cafeTypes.Count();
            var pager = new Pager(count, page, pageSize);
            int skip = (page - 1) * pageSize;
            cafeTypes = cafeTypes.Skip(skip).Take(pager.PageSize);

            var viewModel = new CafeTypeIndexViewModel()
            {
                Pager = pager,
                Sorter = new CafeTypeSorter(sort),
                Filterer = new CafeTypeFilterer(search),
                Data = cafeTypes.ToList()
            };

            return View(viewModel);
        }

        public ActionResult Create()
        {
            try
            {
                if (!_repository.CheckRolePermission(nameof(Helpers.RolePermission.CafesCreate)))
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }
            catch (ArgumentException)
            {
                return RedirectToAction("Logout", "Account");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Create(CafeType newCafeType)
        {
            if (_repository.Get().FirstOrDefault(x => x.Name == newCafeType.Name) != null)
            {
                ModelState.AddModelError("Name", "Такой тип заведения уже существует");
            }

            if (ModelState.IsValid)
            {
                var cafeType = new CafeType
                {
                    CafeTypeId = null,
                    Name = newCafeType.Name
                };

                _repository.Create(cafeType);
                return RedirectToAction("Index");
            }

            return View(newCafeType);
        }

        public ActionResult Edit(int id)
        {
            try
            {
                if (!_repository.CheckRolePermission(nameof(Helpers.RolePermission.CafesUpdate)))
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }
            catch (ArgumentException)
            {
                return RedirectToAction("Logout", "Account");
            }

            CafeType cafeType = _repository.Get(id);
            if (cafeType != null)
                return View(cafeType);
            return NotFound();
        }

        [HttpPost]
        public ActionResult Edit(CafeType newCafeType)
        {
            CafeType cafeType = _repository.Get((int)newCafeType.CafeTypeId);
            if (_repository.Get().FirstOrDefault(x => x.Name == newCafeType.Name && cafeType.Name != newCafeType.Name) != null)
            {
                ModelState.AddModelError("Name", "Такой тип заведения уже существует");
            }

            if (ModelState.IsValid)
            {
                cafeType.Name = newCafeType.Name;
                _repository.Update(cafeType);
                return RedirectToAction("Index");
            }

            return View(newCafeType);
        }

        [HttpGet]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            try
            {
                if (!_repository.CheckRolePermission(nameof(Helpers.RolePermission.CafesDelete)))
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }
            catch (ArgumentException)
            {
                return RedirectToAction("Logout", "Account");
            }

            CafeType cafeType = _repository.Get(id);
            if (cafeType != null)
            {
                return View(cafeType);
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
