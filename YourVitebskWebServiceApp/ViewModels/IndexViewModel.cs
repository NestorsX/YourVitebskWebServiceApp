using System.Collections.Generic;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.ViewModels
{
    public class IndexViewModel<T> : IFilterer, ISorter
    {
        public IEnumerable<T> Data { get; set; }
        public Pager Pager { get; set; }
        public IFilterer Filterer { get; set; }
        public ISorter Sorter { get; set; }
    }
}
