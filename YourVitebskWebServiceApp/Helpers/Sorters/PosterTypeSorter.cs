using YourVitebskWebServiceApp.Helpers.SortStates;

namespace YourVitebskWebServiceApp.Helpers.Sorters
{
    public class PosterTypeSorter
    {
        public PosterTypeSortStates IdSort { get; set; }
        public PosterTypeSortStates NameSort { get; set; }
        public PosterTypeSortStates Current { get; set; }

        public PosterTypeSorter(PosterTypeSortStates sort)
        {
            IdSort = sort == PosterTypeSortStates.PosterTypeIdAsc ? PosterTypeSortStates.PosterTypeIdDesc : PosterTypeSortStates.PosterTypeIdAsc;
            NameSort = sort == PosterTypeSortStates.NameAsc ? PosterTypeSortStates.NameDesc : PosterTypeSortStates.NameAsc;
            Current = sort;
        }
    }
}
