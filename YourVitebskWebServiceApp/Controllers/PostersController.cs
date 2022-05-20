using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;
using YourVitebskWebServiceApp.ViewModels;

namespace YourVitebskWebServiceApp.Controllers
{
    [Authorize]
    public class PostersController : Controller
    {
        private readonly YourVitebskDBContext _context;
        private readonly IRepository<Models.Poster> _repository;

        public PostersController(YourVitebskDBContext context, IRepository<Models.Poster> repository)
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
            ViewBag.PosterTypes = _context.PosterTypes;
            return View();
        }

        [HttpPost]
        public ActionResult Create(PosterViewModel newPoster)
        {
            if (_context.Posters.FirstOrDefault(x => x.Title == newPoster.Title) != null)
            {
                ModelState.AddModelError("Title", "Афиша с таким именем уже используется");
            }

            if (newPoster.PosterTypeId == 0)
            {
                ModelState.AddModelError("PosterTypeId", "Выберите тип искусства");
            }

            if (ModelState.IsValid)
            {
                var Poster = new Models.Poster
                {
                    PosterId = null,
                    PosterTypeId = newPoster.PosterTypeId,
                    Title = newPoster.Title,
                    Description = newPoster.Description,
                    DateTime = newPoster.DateTime,
                    Address = newPoster.Address,
                    ExternalLink = newPoster.ExternalLink ?? ""
                };

                _repository.Create(Poster);
                return RedirectToAction("Index");
            }

            ViewBag.PosterTypes = _context.PosterTypes;
            return View(newPoster);
        }

        public ActionResult Edit(int id)
        {
            Models.Poster poster = _repository.Get(id);
            if (poster != null)
            {
                var viewModel = new PosterViewModel
                {
                    PosterId = poster.PosterId,
                    PosterTypeId = poster.PosterTypeId,
                    Title = poster.Title,
                    Description = poster.Description,
                    DateTime = poster.DateTime,
                    Address = poster.Address,
                    ExternalLink = poster.ExternalLink ?? ""
                };

                ViewBag.PosterTypes = _context.PosterTypes;
                ViewData["PosterTypeId"] = poster.PosterTypeId;
                return View(viewModel);
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult Edit(PosterViewModel newPoster)
        {
            Models.Poster Poster = _repository.Get((int)newPoster.PosterId);
            if (_context.Posters.FirstOrDefault(x => x.Title == newPoster.Title && newPoster.Title != Poster.Title) != null)
            {
                ModelState.AddModelError("Title", "Афиша с таким именем уже используется");
            }

            if (newPoster.PosterTypeId == 0)
            {
                ModelState.AddModelError("PosterTypeId", "Выберите тип искусства");
            }

            if (ModelState.IsValid)
            {
                Poster.PosterTypeId = newPoster.PosterTypeId;
                Poster.Title = newPoster.Title;
                Poster.Description = newPoster.Description;
                Poster.DateTime = newPoster.DateTime;
                Poster.Address = newPoster.Address;
                Poster.ExternalLink = newPoster.ExternalLink;
                _repository.Update(Poster);
                return RedirectToAction("Index");
            }

            ViewBag.PosterTypes = _context.PosterTypes;
            ViewData["PosterTypeId"] = newPoster.PosterTypeId;
            return View(newPoster);
        }

        [HttpGet]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            Models.Poster Poster = _repository.Get(id);
            if (Poster != null)
            {
                ViewData["PosterType"] = _context.PosterTypes.First(x => x.PosterTypeId == Poster.PosterTypeId).Name;
                return View(Poster);
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
