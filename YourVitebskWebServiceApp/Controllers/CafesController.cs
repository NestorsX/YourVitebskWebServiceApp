﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.Controllers
{
    [Authorize]
    public class CafesController : Controller
    {
        private readonly YourVitebskDBContext _context;
        private readonly IImageRepository<Cafe> _repository;

        public CafesController(YourVitebskDBContext context, IImageRepository<Cafe> repository)
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
            ViewBag.CafeTypes = _context.CafeTypes;
            return View();
        }

        [HttpPost]
        public ActionResult Create(Cafe newCafe, IFormFileCollection uploadedFiles)
        {
            if (_context.Cafes.FirstOrDefault(x => x.Title == newCafe.Title) != null)
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

            ViewBag.CafeTypes = _context.CafeTypes;
            return View(newCafe);
        }

        public ActionResult Edit(int id)
        {
            Cafe cafe = (Cafe)_repository.Get(id);
            if (cafe != null)
            {
                ViewBag.CafeTypes = _context.CafeTypes;
                return View(cafe);
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult Edit(Cafe newCafe, IFormFileCollection uploadedFiles)
        {
            Cafe cafe = (Cafe)_repository.Get((int)newCafe.CafeId);
            if (_context.Cafes.FirstOrDefault(x => x.Title == newCafe.Title && newCafe.Title != cafe.Title) != null)
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

            ViewBag.CafeTypes = _context.CafeTypes;
            return View(newCafe);
        }

        [HttpGet]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            Cafe cafe = (Cafe)_repository.Get(id);
            if (cafe != null)
            {
                ViewData["CafeType"] = _context.CafeTypes.First(x => x.CafeTypeId == cafe.CafeTypeId).Name;
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
