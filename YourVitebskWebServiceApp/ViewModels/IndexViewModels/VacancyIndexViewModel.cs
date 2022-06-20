using System.Collections.Generic;
using YourVitebskWebServiceApp.Helpers.Filterers;
using YourVitebskWebServiceApp.Helpers.Sorters;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.ViewModels.IndexViewModels
{
    public class VacancyIndexViewModel
    {
        public IEnumerable<Vacancy> Data { get; set; }
        public Pager Pager { get; set; }
        public VacancyFilterer Filterer { get; set; }
        public VacancySorter Sorter { get; set; }
    }
}
