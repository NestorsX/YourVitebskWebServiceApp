using System.Collections.Generic;
using YourVitebskWebServiceApp.Helpers.Filterers;
using YourVitebskWebServiceApp.Helpers.Sorters;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.ViewModels.IndexViewModels
{
    public class PosterIndexViewModel
    {
        public IEnumerable<PosterViewModel> Data { get; set; }
        public Pager Pager { get; set; }
        public PosterFilterer Filterer { get; set; }
        public PosterSorter Sorter { get; set; }
    }
}
