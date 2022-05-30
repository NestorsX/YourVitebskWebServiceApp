using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.Controllers
{
    [Authorize]
    public class BusShedulesController : Controller
    {
        private readonly YourVitebskDBContext _context;
        private readonly IRepository<BusShedule> _repository;

        public BusShedulesController(YourVitebskDBContext context, IRepository<BusShedule> repository)
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
            ViewBag.Buses = _context.Buses;
            ViewBag.BusStops = _context.BusStops;
            return View();
        }

        [HttpPost]
        public ActionResult Create(BusShedule newBusShedule)
        {
            if (_context.BusShedules.FirstOrDefault(x => x.BusId == newBusShedule.BusId 
                                                      && x.BusRoute == newBusShedule.BusRoute 
                                                      && x.BusStopNumber == newBusShedule.BusStopNumber) != null)
            {
                ModelState.AddModelError("BusStopNumber", "Этот номер остановки в маршруте уже существует");
            }

            if (ModelState.IsValid)
            {
                var busShedule = new BusShedule
                {
                    BusSheduleId = null,
                    BusId = newBusShedule.BusId,
                    BusStopId = newBusShedule.BusStopId,
                    BusRoute = newBusShedule.BusRoute,
                    BusStopNumber = newBusShedule.BusStopNumber,
                    WorkDayShedule = newBusShedule.WorkDayShedule,
                    DayOffShedule = newBusShedule.DayOffShedule
                };

                _repository.Create(busShedule);
                return RedirectToAction("Index");
            }

            ViewBag.Buses = _context.Buses;
            ViewBag.BusStops = _context.BusStops;
            return View(newBusShedule);
        }

        public ActionResult Edit(int id)
        {
            BusShedule busShedule = _repository.Get(id);
            if (busShedule != null)
            {
                ViewBag.Buses = _context.Buses;
                ViewBag.BusStops = _context.BusStops;
                return View(busShedule);
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult Edit(BusShedule newBusShedule)
        {
            BusShedule busShedule = _repository.Get((int)newBusShedule.BusSheduleId);
            if (_context.BusShedules.FirstOrDefault(x => x.BusId == newBusShedule.BusId 
                                                      && x.BusRoute == newBusShedule.BusRoute 
                                                      && x.BusStopNumber == newBusShedule.BusStopNumber) != null)
            {
                ModelState.AddModelError("BusStopNumber", "Этот номер остановки в маршруте уже существует");
            }

            if (ModelState.IsValid)
            {
                busShedule.BusId = newBusShedule.BusId;
                busShedule.BusStopId = newBusShedule.BusStopId;
                busShedule.BusRoute = newBusShedule.BusRoute;
                busShedule.BusStopNumber = newBusShedule.BusStopNumber;
                busShedule.WorkDayShedule = newBusShedule.WorkDayShedule;
                busShedule.DayOffShedule = newBusShedule.DayOffShedule;
                _repository.Update(busShedule);
                return RedirectToAction("Index");
            }

            ViewBag.Buses = _context.Buses;
            ViewBag.BusStops = _context.BusStops;
            return View(newBusShedule);
        }

        [HttpGet]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            BusShedule busShedule = _repository.Get(id);
            if (busShedule != null)
            {
                ViewData["BusNumber"] = _context.Buses.First(x => x.BusId == busShedule.BusId).Number;
                ViewData["BusStopName"] = _context.BusStops.First(x => x.BusStopId == busShedule.BusStopId).Name;
                return View(busShedule);
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
