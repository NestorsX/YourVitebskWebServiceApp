using System.Collections.Generic;
using YourVitebskWebServiceApp.Helpers.Filterers;
using YourVitebskWebServiceApp.Helpers.Sorters;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.ViewModels.IndexViewModels
{
    public class CafeTypeIndexViewModel
    {
        public IEnumerable<CafeType> Data { get; set; }
        public Pager Pager { get; set; }
        public CafeTypeFilterer Filterer { get; set; }
        public CafeTypeSorter Sorter { get; set; }
    }
}
