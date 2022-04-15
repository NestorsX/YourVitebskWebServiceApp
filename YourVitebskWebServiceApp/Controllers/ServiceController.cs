using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.Controllers
{
    [Authorize]
    public class ServiceController : Controller
    {
        private readonly YourVitebskDBContext _context;
        private readonly IRepository<Service> _repository;

        public ServiceController(YourVitebskDBContext context, IRepository<Service> repository)
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
        public IActionResult CreateAsync(Service newService)
        {
            if (_context.Services.FirstOrDefault(x => x.Name == newService.Name) != null)
            {
                ModelState.AddModelError("Name", "Такой сервис уже существует");
            }

            if (ModelState.IsValid)
            {
                var service = new Service
                {
                    ServiceId = null,
                    Name = newService.Name
                };

                _repository.Create(service);
                return RedirectToAction("Index");
            }

            return View(newService);
        }

        public ActionResult Edit(int id)
        {
            Service service = _repository.Get(id);
            if (service != null)
                return View(service);
            return NotFound();
        }

        [HttpPost]
        public ActionResult Edit(Service newService)
        {
            Service service = _repository.Get((int)newService.ServiceId);

            if (_context.Services.FirstOrDefault(x => x.Name == newService.Name && service.Name != newService.Name) != null)
            {
                ModelState.AddModelError("Name", "Такой сервис уже существует");
            }

            if (ModelState.IsValid)
            {
                service.Name = newService.Name;
                _repository.Update(service);
                return RedirectToAction("Index");
            }

            return View(newService);
        }

        [HttpGet]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            Service service = _repository.Get(id);
            if (service != null)
            {
                return View(service);
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
