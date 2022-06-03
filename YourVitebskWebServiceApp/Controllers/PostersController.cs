using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IImageRepository<Poster> _repository;

        public PostersController(YourVitebskDBContext context, IImageRepository<Poster> repository)
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
        public ActionResult Create(PosterViewModel newPoster, IFormFileCollection uploadedFiles)
        {
            if (_context.Posters.FirstOrDefault(x => x.Title == newPoster.Title) != null)
            {
                ModelState.AddModelError("Title", "Афиша с таким именем уже используется");
            }

            if (ModelState.IsValid)
            {
                var poster = new Poster
                {
                    PosterId = null,
                    PosterTypeId = newPoster.PosterTypeId,
                    Title = newPoster.Title,
                    Description = newPoster.Description,
                    DateTime = newPoster.DateTime,
                    Address = newPoster.Address,
                    ExternalLink = newPoster.ExternalLink
                };

                _repository.Create(poster, uploadedFiles);
                return RedirectToAction("Index");
            }

            ViewBag.PosterTypes = _context.PosterTypes;
            return View(newPoster);
        }

        public ActionResult Edit(int id)
        {
            Poster poster = (Poster)_repository.Get(id);
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
                    ExternalLink = poster.ExternalLink
                };

                ViewBag.PosterTypes = _context.PosterTypes;
                ViewData["PosterTypeId"] = poster.PosterTypeId;
                return View(viewModel);
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult Edit(PosterViewModel newPoster, IFormFileCollection uploadedFiles)
        {
            Poster poster = (Poster)_repository.Get((int)newPoster.PosterId);
            if (_context.Posters.FirstOrDefault(x => x.Title == newPoster.Title && newPoster.Title != poster.Title) != null)
            {
                ModelState.AddModelError("Title", "Афиша с таким именем уже используется");
            }

            if (ModelState.IsValid)
            {
                poster.PosterTypeId = newPoster.PosterTypeId;
                poster.Title = newPoster.Title;
                poster.Description = newPoster.Description;
                poster.DateTime = newPoster.DateTime;
                poster.Address = newPoster.Address;
                poster.ExternalLink = newPoster.ExternalLink;
                _repository.Update(poster, uploadedFiles);
                return RedirectToAction("Index");
            }

            ViewBag.PosterTypes = _context.PosterTypes;
            return View(newPoster);
        }

        [HttpGet]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            Poster poster = (Poster)_repository.Get(id);
            if (poster != null)
            {
                ViewData["PosterType"] = _context.PosterTypes.First(x => x.PosterTypeId == poster.PosterTypeId).Name;
                return View(poster);
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
