using YourVitebskWebServiceApp.Helpers.SortStates;

namespace YourVitebskWebServiceApp.Helpers.Sorters
{
    public class CafeTypeSorter
    {
        public CafeTypeSortStates IdSort { get; set; }
        public CafeTypeSortStates NameSort { get; set; }
        public CafeTypeSortStates Current { get; set; }

        public CafeTypeSorter(CafeTypeSortStates sort)
        {
            IdSort = sort == CafeTypeSortStates.CafeTypeIdAsc ? CafeTypeSortStates.CafeTypeIdDesc : CafeTypeSortStates.CafeTypeIdAsc;
            NameSort = sort == CafeTypeSortStates.NameAsc ? CafeTypeSortStates.NameDesc : CafeTypeSortStates.NameAsc;
            Current = sort;
        }
    }
}
