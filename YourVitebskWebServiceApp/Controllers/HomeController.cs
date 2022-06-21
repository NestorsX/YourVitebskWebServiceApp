using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourVitebskWebServiceApp.Interfaces;

namespace YourVitebskWebServiceApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IHomeRepository _repository;

        public HomeController(YourVitebskDBContext context, IHomeRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            return View(_repository.Get(HttpContext.User.Identity.Name));
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
