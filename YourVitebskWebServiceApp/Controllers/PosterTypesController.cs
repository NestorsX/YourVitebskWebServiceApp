using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.Controllers
{
    [Authorize]
    public class PosterTypesController : Controller
    {
        private readonly YourVitebskDBContext _context;
        private readonly IRepository<PosterType> _repository;

        public PosterTypesController(YourVitebskDBContext context, IRepository<PosterType> repository)
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
        public IActionResult CreateAsync(PosterType newPosterType)
        {
            if (_context.PosterTypes.FirstOrDefault(x => x.Name == newPosterType.Name) != null)
            {
                ModelState.AddModelError("Name", "Такой тип искусства уже существует");
            }

            if (ModelState.IsValid)
            {
                var posterType = new PosterType
                {
                    PosterTypeId = null,
                    Name = newPosterType.Name
                };

                _repository.Create(posterType);
                return RedirectToAction("Index");
            }

            return View(newPosterType);
        }

        public ActionResult Edit(int id)
        {
            PosterType posterType = _repository.Get(id);
            if (posterType != null)
                return View(posterType);
            return NotFound();
        }

        [HttpPost]
        public ActionResult Edit(PosterType newPosterType)
        {
            PosterType posterType = _repository.Get((int)newPosterType.PosterTypeId);
            if (_context.PosterTypes.FirstOrDefault(x => x.Name == newPosterType.Name && posterType.Name != newPosterType.Name) != null)
            {
                ModelState.AddModelError("Name", "Такой тип искусства уже существует");
            }

            if (ModelState.IsValid)
            {
                posterType.Name = newPosterType.Name;
                _repository.Update(posterType);
                return RedirectToAction("Index");
            }

            return View(newPosterType);
        }

        [HttpGet]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            PosterType posterType = _repository.Get(id);
            if (posterType != null)
            {
                return View(posterType);
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
