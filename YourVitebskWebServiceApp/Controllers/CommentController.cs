using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
