﻿using Microsoft.AspNetCore.Authorization;
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
    public class PosterTypesController : Controller
    {
        private readonly YourVitebskDBContext _context;
        private readonly IRepository<PosterType> _repository;

        public PosterTypesController(YourVitebskDBContext context, IRepository<PosterType> repository)
        {
            _context = context;
            _repository = repository;
        }

        public ActionResult Index(string search, PosterTypeSortStates sort = PosterTypeSortStates.PosterTypeIdAsc, int page = 1)
        {
            var posterTypes = (IEnumerable<PosterType>)_repository.Get();
            if (!string.IsNullOrWhiteSpace(search))
            {
                posterTypes = posterTypes.Where(x => x.PosterTypeId.ToString().Contains(search) || x.Name.ToLower().Contains(search.ToLower()));
            }

            posterTypes = sort switch
            {
                PosterTypeSortStates.PosterTypeIdDesc => posterTypes.OrderByDescending(x => x.PosterTypeId),
                PosterTypeSortStates.NameAsc => posterTypes.OrderBy(x => x.Name),
                PosterTypeSortStates.NameDesc => posterTypes.OrderByDescending(x => x.Name),
                _ => posterTypes.OrderBy(x => x.PosterTypeId),
            };

            const int pageSize = 5;
            if (page < 1)
            {
                page = 1;
            }

            int count = posterTypes.Count();
            var pager = new Pager(count, page, pageSize);
            int skip = (page - 1) * pageSize;
            posterTypes = posterTypes.Skip(skip).Take(pager.PageSize);

            var viewModel = new PosterTypeIndexViewModel()
            {
                Pager = pager,
                Sorter = new PosterTypeSorter(sort),
                Filterer = new PosterTypeFilterer(search),
                Data = posterTypes.ToList()
            };

            return View(viewModel);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(PosterType newPosterType)
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
