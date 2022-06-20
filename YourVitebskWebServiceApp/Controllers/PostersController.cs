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
    public class PostersController : Controller
    {
        private readonly YourVitebskDBContext _context;
        private readonly IImageRepository<Poster> _repository;

        public PostersController(YourVitebskDBContext context, IImageRepository<Poster> repository)
        {
            _context = context;
            _repository = repository;
        }

        public ActionResult Index(int? cafeType, string search, PosterSortStates sort = PosterSortStates.PosterIdAsc, int page = 1)
        {
            try
            {
                if (!_repository.CheckRolePermission(HttpContext.User.Identity.Name, nameof(Helpers.RolePermission.PostersGet)))
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }
            catch (ArgumentException)
            {
                return RedirectToAction("Logout", "Account");
            }

            var posters = (IEnumerable<PosterViewModel>)_repository.Get();
            if (cafeType != null && cafeType != 0)
            {
                posters = posters.Where(x => x.PosterTypeId == cafeType);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                posters = posters.Where(x => x.PosterId.ToString().Contains(search) ||
                                         x.PosterType.ToLower().Contains(search.ToLower()) ||
                                         x.Title.ToLower().Contains(search.ToLower())
                );
            }

            posters = sort switch
            {
                PosterSortStates.PosterIdDesc => posters.OrderByDescending(x => x.PosterId),
                PosterSortStates.PosterTypeAsc => posters.OrderBy(x => x.PosterType),
                PosterSortStates.PosterTypeDesc => posters.OrderByDescending(x => x.PosterType),
                PosterSortStates.TitleAsc => posters.OrderBy(x => x.Title),
                PosterSortStates.TitleDesc => posters.OrderByDescending(x => x.Title),
                _ => posters.OrderBy(x => x.PosterId),
            };

            const int pageSize = 5;
            if (page < 1)
            {
                page = 1;
            }

            int count = posters.Count();
            var pager = new Pager(count, page, pageSize);
            int skip = (page - 1) * pageSize;
            posters = posters.Skip(skip).Take(pager.PageSize);

            var viewModel = new PosterIndexViewModel()
            {
                Pager = pager,
                Sorter = new PosterSorter(sort),
                Filterer = new PosterFilterer(_context.PosterTypes.ToList(), cafeType, search),
                Data = posters.ToList()
            };

            return View(viewModel);
        }

        public ActionResult Create()
        {
            try
            {
                if (!_repository.CheckRolePermission(HttpContext.User.Identity.Name, nameof(Helpers.RolePermission.PostersCreate)))
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }
            catch (ArgumentException)
            {
                return RedirectToAction("Logout", "Account");
            }

            ViewBag.PosterTypes = _context.PosterTypes;
            return View();
        }

        [HttpPost]
        public ActionResult Create(Poster newPoster, IFormFileCollection uploadedFiles)
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
            try
            {
                if (!_repository.CheckRolePermission(HttpContext.User.Identity.Name, nameof(Helpers.RolePermission.PostersUpdate)))
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }
            catch (ArgumentException)
            {
                return RedirectToAction("Logout", "Account");
            }

            Poster poster = (Poster)_repository.Get(id);
            if (poster != null)
            {
                ViewBag.PosterTypes = _context.PosterTypes;
                return View(poster);
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult Edit(Poster newPoster, IFormFileCollection uploadedFiles)
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
            try
            {
                if (!_repository.CheckRolePermission(HttpContext.User.Identity.Name, nameof(Helpers.RolePermission.PostersDelete)))
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }
            catch (ArgumentException)
            {
                return RedirectToAction("Logout", "Account");
            }

            Poster poster = (Poster)_repository.Get(id);
            if (poster != null)
            {
                ViewBag.PosterType = _context.PosterTypes.First(x => x.PosterTypeId == poster.PosterTypeId).Name;
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
