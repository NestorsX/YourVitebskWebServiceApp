using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.Controllers
{
    [Authorize]
    public class BusesController : Controller
    {
        private readonly YourVitebskDBContext _context;
        private readonly IRepository<Bus> _repository;

        public BusesController(YourVitebskDBContext context, IRepository<Bus> repository)
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
        public ActionResult Create(Bus newBus)
        {
            if (_context.Buses.FirstOrDefault(x => x.Number == newBus.Number) != null)
            {
                ModelState.AddModelError("Number", "Такой автобус уже существует");
            }

            if (ModelState.IsValid)
            {
                var bus = new Bus
                {
                    BusId = null,
                    Number = newBus.Number
                };

                _repository.Create(bus);
                return RedirectToAction("Index");
            }

            return View(newBus);
        }

        public ActionResult Edit(int id)
        {
            if (id == 1 || id == 2)
            {
                return RedirectToAction("Index");
            }

            Bus bus = _repository.Get(id);
            if (bus != null)
                return View(bus);
            return NotFound();
        }

        [HttpPost]
        public ActionResult Edit(Bus newBus)
        {
            Bus bus = _repository.Get((int)newBus.BusId);
            if (_context.Buses.FirstOrDefault(x => x.Number == newBus.Number && bus.Number != newBus.Number) != null)
            {
                ModelState.AddModelError("Number", "Такой автобус уже существует");
            }

            if (ModelState.IsValid)
            {
                bus.Number = newBus.Number;
                _repository.Update(bus);
                return RedirectToAction("Index");
            }

            return View(newBus);
        }

        [HttpGet]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            if (id == 1 || id == 2)
            {
                return RedirectToAction("Index");
            }

            Bus bus = _repository.Get(id);
            if (bus != null)
            {
                return View(bus);
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
