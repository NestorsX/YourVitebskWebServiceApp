using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.Controllers
{
    [Authorize]
    public class VacanciesController : Controller
    {
        private readonly YourVitebskDBContext _context;
        private readonly IRepository<Vacancy> _repository;

        public VacanciesController(YourVitebskDBContext context, IRepository<Vacancy> repository)
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
            return View();
        }

        [HttpPost]
        public ActionResult Create(Vacancy newVacancy)
        {
            if (ModelState.IsValid)
            {
                var vacancy = new Vacancy
                {
                    VacancyId = null,
                    Title = newVacancy.Title,
                    Description = newVacancy.Description,
                    Salary = newVacancy.Salary,
                    CompanyName = newVacancy.CompanyName,
                    Contacts = newVacancy.Contacts,
                    Address = newVacancy.Address,
                    PublishDate = DateTime.Now
                };

                _repository.Create(vacancy);
                return RedirectToAction("Index");
            }

            return View(newVacancy);
        }

        public ActionResult Edit(int id)
        {
            Vacancy vacancy = _repository.Get(id);
            if (vacancy != null)
            {
                return View(vacancy);
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult Edit(Vacancy newVacancy)
        {
            Vacancy vacancy = _repository.Get((int)newVacancy.VacancyId);
            if (ModelState.IsValid)
            {
                vacancy.Title = newVacancy.Title;
                vacancy.Description = newVacancy.Description;
                vacancy.Salary = newVacancy.Salary;
                vacancy.CompanyName = newVacancy.CompanyName;
                vacancy.Contacts = newVacancy.Contacts;
                vacancy.Address = newVacancy.Address;
                _repository.Update(vacancy);
                return RedirectToAction("Index");
            }

            return View(newVacancy);
        }

        [HttpGet]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            Vacancy vacancy = _repository.Get(id);
            if (vacancy != null)
            {
                return View(vacancy);
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
