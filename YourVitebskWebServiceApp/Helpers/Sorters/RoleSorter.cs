using YourVitebskWebServiceApp.Helpers.SortStates;

namespace YourVitebskWebServiceApp.Helpers.Sorters
{
    public class RoleSorter
    {
        public RoleSortStates IdSort { get; set; }
        public RoleSortStates NameSort { get; set; }
        public RoleSortStates Current { get; set; }

        public RoleSorter(RoleSortStates sort)
        {
            IdSort = sort == RoleSortStates.RoleIdAsc ? RoleSortStates.RoleIdDesc : RoleSortStates.RoleIdAsc;
            NameSort = sort == RoleSortStates.NameAsc ? RoleSortStates.NameDesc : RoleSortStates.NameAsc;
            Current = sort;
        }
    }
}
