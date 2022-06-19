using YourVitebskWebServiceApp.Helpers.SortStates;

namespace YourVitebskWebServiceApp.Helpers.Sorters
{
    public class UserSorter
    {
        public UserSortStates IdSort { get; set; }
        public UserSortStates RoleSort { get; set; }
        public UserSortStates EmailSort { get; set; }
        public UserSortStates FirstNameSort { get; set; }
        public UserSortStates LastNameSort { get; set; }
        public UserSortStates PhoneNumberSort { get; set; }
        public UserSortStates VisibleSort { get; set; }
        public UserSortStates Current { get; set; }

        public UserSorter(UserSortStates sort)
        {
            IdSort = sort == UserSortStates.UserIdAsc ? UserSortStates.UserIdDesc : UserSortStates.UserIdAsc;
            RoleSort = sort == UserSortStates.RoleAsc ? UserSortStates.RoleDesc : UserSortStates.RoleAsc;
            EmailSort = sort == UserSortStates.EmailAsc ? UserSortStates.EmailDesc : UserSortStates.EmailAsc;
            FirstNameSort = sort == UserSortStates.FirstNameAsc ? UserSortStates.FirstNameDesc : UserSortStates.FirstNameAsc;
            LastNameSort = sort == UserSortStates.LastNameAsc ? UserSortStates.LastNameDesc : UserSortStates.LastNameAsc;
            PhoneNumberSort = sort == UserSortStates.PhoneNumberAsc ? UserSortStates.PhoneNumberDesc : UserSortStates.PhoneNumberAsc;
            VisibleSort = sort == UserSortStates.VisibleAsc ? UserSortStates.VisibleDesc : UserSortStates.VisibleAsc;
            Current = sort;
        }
    }
}
