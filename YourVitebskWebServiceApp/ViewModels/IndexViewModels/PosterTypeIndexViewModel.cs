using System.Collections.Generic;
using YourVitebskWebServiceApp.Helpers.Filterers;
using YourVitebskWebServiceApp.Helpers.Sorters;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.ViewModels.IndexViewModels
{
    public class PosterTypeIndexViewModel
    {
        public IEnumerable<PosterType> Data { get; set; }
        public Pager Pager { get; set; }
        public PosterTypeFilterer Filterer { get; set; }
        public PosterTypeSorter Sorter { get; set; }
    }
}
