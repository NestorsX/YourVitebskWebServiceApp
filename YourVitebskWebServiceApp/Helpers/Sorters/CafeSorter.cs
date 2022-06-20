using YourVitebskWebServiceApp.Helpers.SortStates;

namespace YourVitebskWebServiceApp.Helpers.Sorters
{
    public class CafeSorter
    {
        public CafeSortStates IdSort { get; set; }
        public CafeSortStates CafeTypeSort { get; set; }
        public CafeSortStates TitleSort { get; set; }
        public CafeSortStates Current { get; set; }

        public CafeSorter(CafeSortStates sort)
        {
            IdSort = sort == CafeSortStates.CafeIdAsc ? CafeSortStates.CafeIdDesc : CafeSortStates.CafeIdAsc;
            CafeTypeSort = sort == CafeSortStates.CafeTypeAsc ? CafeSortStates.CafeTypeDesc : CafeSortStates.CafeTypeAsc;
            TitleSort = sort == CafeSortStates.TitleAsc ? CafeSortStates.TitleDesc : CafeSortStates.TitleAsc;
            Current = sort;
        }
    }
}
