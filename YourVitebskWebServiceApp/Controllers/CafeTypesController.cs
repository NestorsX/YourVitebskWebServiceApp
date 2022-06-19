using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        private readonly YourVitebskDBContext _context;
        private readonly IRepository<CafeType> _repository;

        public CafeTypesController(YourVitebskDBContext context, IRepository<CafeType> repository)
        {
            _context = context;
            _repository = repository;
        }

        public ActionResult Index(string search, CafeTypeSortStates sort = CafeTypeSortStates.CafeTypeIdAsc, int page = 1)
        {
            var cafeTypes = (IEnumerable<CafeType>)_repository.Get();
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
            return View();
        }

        [HttpPost]
        public ActionResult Create(CafeType newCafeType)
        {
            if (_context.CafeTypes.FirstOrDefault(x => x.Name == newCafeType.Name) != null)
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
            CafeType cafeType = _repository.Get(id);
            if (cafeType != null)
                return View(cafeType);
            return NotFound();
        }

        [HttpPost]
        public ActionResult Edit(CafeType newCafeType)
        {
            CafeType cafeType = _repository.Get((int)newCafeType.CafeTypeId);
            if (_context.CafeTypes.FirstOrDefault(x => x.Name == newCafeType.Name && cafeType.Name != newCafeType.Name) != null)
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
