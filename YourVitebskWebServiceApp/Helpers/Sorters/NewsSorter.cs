using YourVitebskWebServiceApp.Helpers.SortStates;

namespace YourVitebskWebServiceApp.Helpers.Sorters
{
    public class NewsSorter
    {
        public NewsSortStates IdSort { get; set; }
        public NewsSortStates Current { get; set; }

        public NewsSorter(NewsSortStates sort)
        {
            IdSort = sort == NewsSortStates.NewsIdAsc ? NewsSortStates.NewsIdDesc : NewsSortStates.NewsIdAsc;
            Current = sort;
        }
    }
}
