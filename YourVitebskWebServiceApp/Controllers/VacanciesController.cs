using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
    public class VacanciesController : Controller
    {
        private readonly YourVitebskDBContext _context;
        private readonly IRepository<Vacancy> _repository;

        public VacanciesController(YourVitebskDBContext context, IRepository<Vacancy> repository)
        {
            _context = context;
            _repository = repository;
        }

        public ActionResult Index(string search, VacancySortStates sort = VacancySortStates.VacancyIdAsc, int page = 1)
        {
            try
            {
                if (!_repository.CheckRolePermission(HttpContext.User.Identity.Name, nameof(Helpers.RolePermission.VacanciesGet)))
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }
            catch (ArgumentException)
            {
                return RedirectToAction("Logout", "Account");
            }

            var vacancies = (IEnumerable<Vacancy>)_repository.Get();
            if (!string.IsNullOrWhiteSpace(search))
            {
                vacancies = vacancies.Where(x => x.VacancyId.ToString().Contains(search) ||
                                         x.Title.ToLower().Contains(search.ToLower()) ||
                                         x.CompanyName.ToLower().Contains(search.ToLower()) ||
                                         x.Salary.ToLower().Contains(search.ToLower()) ||
                                         x.PublishDate.ToString("D", new System.Globalization.CultureInfo("ru-RU")).ToLower().Contains(search.ToLower())
                );
            }

            vacancies = sort switch
            {
                VacancySortStates.VacancyIdDesc => vacancies.OrderByDescending(x => x.VacancyId),
                VacancySortStates.TitleAsc => vacancies.OrderBy(x => x.Title),
                VacancySortStates.TitleDesc => vacancies.OrderByDescending(x => x.Title),
                VacancySortStates.SalaryAsc => vacancies.OrderBy(x => x.Salary),
                VacancySortStates.SalaryDesc => vacancies.OrderByDescending(x => x.Salary),
                VacancySortStates.CompanyAsc => vacancies.OrderBy(x => x.CompanyName),
                VacancySortStates.CompanyDesc => vacancies.OrderByDescending(x => x.CompanyName),
                VacancySortStates.DateAsc => vacancies.OrderBy(x => x.PublishDate),
                VacancySortStates.DateDesc => vacancies.OrderByDescending(x => x.PublishDate),
                _ => vacancies.OrderBy(x => x.VacancyId),
            };

            const int pageSize = 5;
            if (page < 1)
            {
                page = 1;
            }

            int count = vacancies.Count();
            var pager = new Pager(count, page, pageSize);
            int skip = (page - 1) * pageSize;
            vacancies = vacancies.Skip(skip).Take(pager.PageSize);

            var viewModel = new VacancyIndexViewModel()
            {
                Pager = pager,
                Sorter = new VacancySorter(sort),
                Filterer = new VacancyFilterer(search),
                Data = vacancies.ToList()
            };

            return View(viewModel);
        }

        public ActionResult Create()
        {
            try
            {
                if (!_repository.CheckRolePermission(HttpContext.User.Identity.Name, nameof(Helpers.RolePermission.VacanciesCreate)))
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }
            catch (ArgumentException)
            {
                return RedirectToAction("Logout", "Account");
            }

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
                    Requirements = newVacancy.Requirements,
                    Conditions = newVacancy.Conditions,
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
            try
            {
                if (!_repository.CheckRolePermission(HttpContext.User.Identity.Name, nameof(Helpers.RolePermission.VacanciesUpdate)))
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }
            catch (ArgumentException)
            {
                return RedirectToAction("Logout", "Account");
            }

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
                vacancy.Requirements = newVacancy.Requirements;
                vacancy.Conditions = newVacancy.Conditions;
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
            try
            {
                if (!_repository.CheckRolePermission(HttpContext.User.Identity.Name, nameof(Helpers.RolePermission.VacanciesDelete)))
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }
            catch (ArgumentException)
            {
                return RedirectToAction("Logout", "Account");
            }

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
