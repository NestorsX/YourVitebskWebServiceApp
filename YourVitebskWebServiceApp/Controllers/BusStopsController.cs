using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.Controllers
{
    [Authorize]
    public class BusStopsController : Controller
    {
        private readonly YourVitebskDBContext _context;
        private readonly IRepository<BusStop> _repository;

        public BusStopsController(YourVitebskDBContext context, IRepository<BusStop> repository)
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
        public ActionResult Create(BusStop newBusStop)
        {
            if (_context.BusStops.FirstOrDefault(x => x.Name == newBusStop.Name) != null)
            {
                ModelState.AddModelError("Name", "Такая остановка уже существует");
            }

            if (ModelState.IsValid)
            {
                var busStop = new BusStop
                {
                    BusStopId = null,
                    Name = newBusStop.Name
                };

                _repository.Create(busStop);
                return RedirectToAction("Index");
            }

            return View(newBusStop);
        }

        public ActionResult Edit(int id)
        {
            BusStop busStop = _repository.Get(id);
            if (busStop != null)
                return View(busStop);
            return NotFound();
        }

        [HttpPost]
        public ActionResult Edit(BusStop newBusStop)
        {
            BusStop busStop = _repository.Get((int)newBusStop.BusStopId);
            if (_context.BusStops.FirstOrDefault(x => x.Name == newBusStop.Name && busStop.Name != newBusStop.Name) != null)
            {
                ModelState.AddModelError("Name", "Такая остановка уже существует");
            }

            if (ModelState.IsValid)
            {
                busStop.Name = newBusStop.Name;
                _repository.Update(busStop);
                return RedirectToAction("Index");
            }

            return View(newBusStop);
        }

        [HttpGet]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            BusStop busStop = _repository.Get(id);
            if (busStop != null)
            {
                return View(busStop);
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
