using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;

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

        public IActionResult Index()
        {
            return View(_repository.Get());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateAsync(CafeType newCafeType)
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
