using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;
using YourVitebskWebServiceApp.ViewModels;

namespace YourVitebskWebServiceApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserRepository _repository;

        public HomeController(IUserRepository repository)
        {
            _repository = repository;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View(_repository.GetUsers());
        }

        public ActionResult Details(int id)
        {
            User user = _repository.Get(id);
            if (user != null)
                return View(user);
            return NotFound();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateAsync(UserViewModel uvm)
        {
            var user = new User
            {
                Email = uvm.Email,
                Password = uvm.Password,
                RoleId = 1
            };

            _repository.Create(user);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            User user = _repository.Get(id);
            if (user != null)
                return View(user);
            return NotFound();
        }

        [HttpPost]
        public ActionResult Edit(UserViewModel uvm)
        {
            var user = new User
            {
                UserId = uvm.UserId,
                Email = uvm.Email,
                Password = uvm.Password,
                RoleId = 1
            };

            _repository.Update(user);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            User user = _repository.Get(id);
            if (user != null)
                return View(user);
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
