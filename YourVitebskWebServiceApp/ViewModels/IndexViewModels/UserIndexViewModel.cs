using System.Collections.Generic;
using YourVitebskWebServiceApp.Helpers.Filterers;
using YourVitebskWebServiceApp.Helpers.Sorters;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.ViewModels.IndexViewModels
{
    public class UserIndexViewModel
    {
        public IEnumerable<UserViewModel> Data { get; set; }
        public Pager Pager { get; set; }
        public UserFilterer Filterer { get; set; }
        public UserSorter Sorter { get; set; }
    }
}
