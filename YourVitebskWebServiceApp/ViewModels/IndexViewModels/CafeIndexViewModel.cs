using System.Collections.Generic;
using YourVitebskWebServiceApp.Helpers.Filterers;
using YourVitebskWebServiceApp.Helpers.Sorters;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.ViewModels.IndexViewModels
{
    public class CafeIndexViewModel
    {
        public IEnumerable<CafeViewModel> Data { get; set; }
        public Pager Pager { get; set; }
        public CafeFilterer Filterer { get; set; }
        public CafeSorter Sorter { get; set; }
    }
}
