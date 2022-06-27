using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourVitebskWebServiceApp.Interfaces;

namespace YourVitebskWebServiceApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IHomeRepository _repository;

        public HomeController(IHomeRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            return View(_repository.Get());
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        public new ActionResult NotFound()
        {
            return View();
        }
    }
}
