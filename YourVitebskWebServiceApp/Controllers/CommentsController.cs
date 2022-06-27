using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using YourVitebskWebServiceApp.Helpers.Filterers;
using YourVitebskWebServiceApp.Helpers.Sorters;
using YourVitebskWebServiceApp.Helpers.SortStates;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;
using YourVitebskWebServiceApp.ViewModels;
using YourVitebskWebServiceApp.ViewModels.IndexViewModels;

namespace YourVitebskWebServiceApp.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly ICommentRepository _repository;

        public CommentsController(ICommentRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Index(string service, string search, CommentSortStates sort = CommentSortStates.CommentIdAsc, int page = 1)
        {
            try
            {
                if (!_repository.CheckRolePermission(nameof(Helpers.RolePermission.CommentsGet)))
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }
            catch (ArgumentException)
            {
                return RedirectToAction("Logout", "Account");
            }

            var comments = _repository.Get();
            if (service != null && service != "Все сервисы")
            {
                comments = comments.Where(x => x.Service.Equals(service));
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                comments = comments.Where(x => x.CommentId.ToString().Contains(search) ||
                                         x.UserEmail.ToLower().Contains(search.ToLower()) ||
                                         x.Service.ToLower().Contains(search.ToLower()) ||
                                         x.ItemName.ToLower().Contains(search.ToLower()) ||
                                         x.PublishDate.ToLower().Contains(search.ToLower())
                );
            }

            comments = sort switch
            {
                CommentSortStates.CommentIdDesc => comments.OrderByDescending(x => x.CommentId),
                CommentSortStates.UserAsc => comments.OrderBy(x => x.UserEmail),
                CommentSortStates.UserDesc => comments.OrderByDescending(x => x.UserEmail),
                CommentSortStates.ServiceAsc => comments.OrderBy(x => x.Service),
                CommentSortStates.ServiceDesc => comments.OrderByDescending(x => x.Service),
                CommentSortStates.ItemAsc => comments.OrderBy(x => x.ItemName),
                CommentSortStates.ItemDesc => comments.OrderByDescending(x => x.ItemName),
                CommentSortStates.IsRecommendAsc => comments.OrderBy(x => x.IsRecommend),
                CommentSortStates.IsRecommendDesc => comments.OrderByDescending(x => x.IsRecommend),
                CommentSortStates.PublishDateAsc => comments.OrderBy(x => x.PublishDate),
                CommentSortStates.PublishDateDesc => comments.OrderByDescending(x => x.PublishDate),
                _ => comments.OrderBy(x => x.CommentId),
            };

            const int pageSize = 5;
            if (page < 1)
            {
                page = 1;
            }

            int count = comments.Count();
            var pager = new Pager(count, page, pageSize);
            int skip = (page - 1) * pageSize;
            comments = comments.Skip(skip).Take(pager.PageSize);

            var viewModel = new CommentIndexViewModel()
            {
                Pager = pager,
                Sorter = new CommentSorter(sort),
                Filterer = new CommentFilterer(service, search),
                Data = comments.ToList()
            };

            return View(viewModel);
        }

        [HttpGet]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            try
            {
                if (!_repository.CheckRolePermission(nameof(Helpers.RolePermission.CommentsDelete)))
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }
            catch (ArgumentException)
            {
                return RedirectToAction("Logout", "Account");
            }

            CommentViewModel comment = _repository.Get(id);
            if (comment != null)
            {
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
