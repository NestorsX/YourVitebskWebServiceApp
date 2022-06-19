using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using YourVitebskWebServiceApp.Helpers.Filterers;
using YourVitebskWebServiceApp.Helpers.Sorters;
using YourVitebskWebServiceApp.Helpers.SortStates;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;
using YourVitebskWebServiceApp.ViewModels.IndexViewModels;

namespace YourVitebskWebServiceApp.Controllers
{
    [Authorize]
    public class NewsController : Controller
    {
        private readonly IImageRepository<News> _repository;

        public NewsController(IImageRepository<News> repository)
        {
            _repository = repository;
        }

        public ActionResult Index(string search, NewsSortStates sort = NewsSortStates.NewsIdAsc, int page = 1)
        {
            var news = (IEnumerable<News>)_repository.Get();
            if (!string.IsNullOrWhiteSpace(search))
            {
                news = news.Where(x => x.NewsId.ToString().Contains(search) ||
                                         x.Title.ToLower().Contains(search.ToLower()) ||
                                         x.Description.ToLower().Contains(search.ToLower()) ||
                                         x.ExternalLink.ToLower().Contains(search.ToLower()));
            }

            news = sort switch
            {
                NewsSortStates.NewsIdDesc => news.OrderByDescending(x => x.NewsId),
                _ => news.OrderBy(x => x.NewsId),
            };

            const int pageSize = 5;
            if (page < 1)
            {
                page = 1;
            }

            int count = news.Count();
            var pager = new Pager(count, page, pageSize);
            int skip = (page - 1) * pageSize;
            news = news.Skip(skip).Take(pager.PageSize);

            var viewModel = new NewsIndexViewModel()
            {
                Pager = pager,
                Sorter = new NewsSorter(sort),
                Filterer = new NewsFilterer(search),
                Data = news.ToList()
            };

            return View(viewModel);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(News newNews, IFormFileCollection uploadedFiles)
        {
            if (ModelState.IsValid)
            {
                var news = new News
                {
                    NewsId = null,
                    Title = newNews.Title,
                    Description = newNews.Description,
                    ExternalLink = newNews.ExternalLink
                };

                _repository.Create(news, uploadedFiles);
                return RedirectToAction("Index");
            }

            return View(newNews);
        }

        public ActionResult Edit(int id)
        {
            News news = (News)_repository.Get(id);
            if (news != null)
            {
                return View(news);
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult Edit(News newNews, IFormFileCollection uploadedFiles)
        {
            News news = (News)_repository.Get((int)newNews.NewsId);
            if (ModelState.IsValid)
            {
                news.Title = newNews.Title;
                news.Description = newNews.Description;
                news.ExternalLink = newNews.ExternalLink;
                _repository.Update(news, uploadedFiles);
                return RedirectToAction("Index");
            }

            return View(newNews);
        }

        [HttpGet]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            News news = (News)_repository.Get(id);
            if (news != null)
            {
                return View(news);
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
