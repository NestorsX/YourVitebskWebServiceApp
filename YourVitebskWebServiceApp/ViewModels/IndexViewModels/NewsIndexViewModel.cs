using System.Collections.Generic;
using YourVitebskWebServiceApp.Helpers.Filterers;
using YourVitebskWebServiceApp.Helpers.Sorters;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.ViewModels.IndexViewModels
{
    public class NewsIndexViewModel
    {
        public IEnumerable<News> Data { get; set; }
        public Pager Pager { get; set; }
        public NewsFilterer Filterer { get; set; }
        public NewsSorter Sorter { get; set; }
    }
}
