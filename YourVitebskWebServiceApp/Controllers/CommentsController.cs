using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly YourVitebskDBContext _context;
        private readonly ICommentRepository _repository;

        public CommentsController(YourVitebskDBContext context, ICommentRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        public IActionResult Index()
        {
            ViewBag.Services = _context.Services;
            ViewBag.UserData = _context.UserData;
            return View(_repository.Get());
        }

        [HttpGet]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            Comment comment = _repository.Get(id);
            if (comment != null)
            {
                ViewData["Service"] = _context.Services.First(x => x.ServiceId == comment.ServiceId);
                UserDatum userDatum = _context.UserData.First(x => x.UserDataId == comment.UserDataId);
                ViewData["UserName"] = $"{userDatum.FirstName} {userDatum.LastName}";
                return View(comment);
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
