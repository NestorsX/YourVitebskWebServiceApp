using System.Collections.Generic;
using YourVitebskWebServiceApp.Helpers.Filterers;
using YourVitebskWebServiceApp.Helpers.Sorters;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.ViewModels.IndexViewModels
{
    public class RoleIndexViewModel
    {
        public IEnumerable<RoleViewModel> Data { get; set; }
        public Pager Pager { get; set; }
        public RoleFilterer Filterer { get; set; }
        public RoleSorter Sorter { get; set; }
    }
}
