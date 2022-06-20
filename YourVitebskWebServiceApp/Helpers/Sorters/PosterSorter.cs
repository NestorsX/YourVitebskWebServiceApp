using YourVitebskWebServiceApp.Helpers.SortStates;

namespace YourVitebskWebServiceApp.Helpers.Sorters
{
    public class PosterSorter
    {
        public PosterSortStates IdSort { get; set; }
        public PosterSortStates PosterTypeSort { get; set; }
        public PosterSortStates TitleSort { get; set; }
        public PosterSortStates Current { get; set; }

        public PosterSorter(PosterSortStates sort)
        {
            IdSort = sort == PosterSortStates.PosterIdAsc ? PosterSortStates.PosterIdDesc : PosterSortStates.PosterIdAsc;
            PosterTypeSort = sort == PosterSortStates.PosterTypeAsc ? PosterSortStates.PosterTypeDesc : PosterSortStates.PosterTypeAsc;
            TitleSort = sort == PosterSortStates.TitleAsc ? PosterSortStates.TitleDesc : PosterSortStates.TitleAsc;
            Current = sort;
        }
    }
}
