using System.Collections.Generic;
using YourVitebskWebServiceApp.Helpers.Filterers;
using YourVitebskWebServiceApp.Helpers.Sorters;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.ViewModels.IndexViewModels
{
    public class CommentIndexViewModel
    {
        public IEnumerable<CommentViewModel> Data { get; set; }
        public Pager Pager { get; set; }
        public CommentFilterer Filterer { get; set; }
        public CommentSorter Sorter { get; set; }
    }
}
