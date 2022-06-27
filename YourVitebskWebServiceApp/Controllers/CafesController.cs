using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class CafesController : Controller
    {
        private readonly ICafeRepository _repository;
        private readonly ICafeTypeRepository _cafeTypesRepository;

        public CafesController(ICafeRepository repository, ICafeTypeRepository cafeTypesRepository)
        {
            _repository = repository;
            _cafeTypesRepository = cafeTypesRepository;
        }

        public ActionResult Index(int? cafeType, string search, CafeSortStates sort = CafeSortStates.CafeIdAsc, int page = 1)
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

            var cafes = _repository.Get();
            if (cafeType != null && cafeType != 0)
            {
                cafes = cafes.Where(x => x.CafeTypeId == cafeType);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                cafes = cafes.Where(x => x.CafeId.ToString().Contains(search) ||
                                         x.CafeType.ToLower().Contains(search.ToLower()) ||
                                         x.Title.ToLower().Contains(search.ToLower())
                );
            }

            cafes = sort switch
            {
                CafeSortStates.CafeIdDesc => cafes.OrderByDescending(x => x.CafeId),
                CafeSortStates.CafeTypeAsc => cafes.OrderBy(x => x.CafeType),
                CafeSortStates.CafeTypeDesc => cafes.OrderByDescending(x => x.CafeType),
                CafeSortStates.TitleAsc => cafes.OrderBy(x => x.Title),
                CafeSortStates.TitleDesc => cafes.OrderByDescending(x => x.Title),
                _ => cafes.OrderBy(x => x.CafeId),
            };

            const int pageSize = 5;
            if (page < 1)
            {
                page = 1;
            }

            int count = cafes.Count();
            var pager = new Pager(count, page, pageSize);
            int skip = (page - 1) * pageSize;
            cafes = cafes.Skip(skip).Take(pager.PageSize);

            var viewModel = new CafeIndexViewModel()
            {
                Pager = pager,
                Sorter = new CafeSorter(sort),
                Filterer = new CafeFilterer(_cafeTypesRepository.Get().ToList(), cafeType, search),
                Data = cafes.ToList()
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

            ViewBag.CafeTypes = _cafeTypesRepository.Get();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Cafe newCafe, IFormFileCollection uploadedFiles)
        {
            if (_repository.Get().FirstOrDefault(x => x.Title == newCafe.Title) != null)
            {
                ModelState.AddModelError("Title", "Заведение с таким именем уже используется");
            }

            if (ModelState.IsValid)
            {
                var cafe = new Cafe
                {
                    CafeId = null,
                    CafeTypeId = newCafe.CafeTypeId,
                    Title = newCafe.Title,
                    Description = newCafe.Description,
                    WorkingTime = newCafe.WorkingTime,
                    Address = newCafe.Address,
                    ExternalLink = newCafe.ExternalLink,
                };

                _repository.Create(cafe, uploadedFiles);
                return RedirectToAction("Index");
            }

            ViewBag.CafeTypes = _cafeTypesRepository.Get();
            return View(newCafe);
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

            Cafe cafe = _repository.Get(id);
            if (cafe != null)
            {
                ViewBag.CafeTypes = _cafeTypesRepository.Get();
                return View(cafe);
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult Edit(Cafe newCafe, IFormFileCollection uploadedFiles)
        {
            Cafe cafe = _repository.Get((int)newCafe.CafeId);
            if (_repository.Get().FirstOrDefault(x => x.Title == newCafe.Title && newCafe.Title != cafe.Title) != null)
            {
                ModelState.AddModelError("Title", "Заведение с таким именем уже существует");
            }

            if (ModelState.IsValid)
            {
                cafe.CafeTypeId = newCafe.CafeTypeId;
                cafe.Title = newCafe.Title;
                cafe.Description = newCafe.Description;
                cafe.WorkingTime = newCafe.WorkingTime;
                cafe.Address = newCafe.Address;
                cafe.ExternalLink = newCafe.ExternalLink;
                _repository.Update(cafe, uploadedFiles);
                return RedirectToAction("Index");
            }

            ViewBag.CafeTypes = _cafeTypesRepository.Get();
            return View(newCafe);
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

            Cafe cafe = _repository.Get(id);
            if (cafe != null)
            {
                ViewBag.CafeType = _cafeTypesRepository.Get().First(x => x.CafeTypeId == cafe.CafeTypeId).Name;
                return View(cafe);
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
