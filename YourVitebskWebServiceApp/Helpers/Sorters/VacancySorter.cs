using YourVitebskWebServiceApp.Helpers.SortStates;

namespace YourVitebskWebServiceApp.Helpers.Sorters
{
    public class VacancySorter
    {
        public VacancySortStates IdSort { get; set; }
        public VacancySortStates TitleSort { get; set; }
        public VacancySortStates SalarySort { get; set; }
        public VacancySortStates CompanySort { get; set; }
        public VacancySortStates DateSort { get; set; }
        public VacancySortStates Current { get; set; }

        public VacancySorter(VacancySortStates sort)
        {
            IdSort = sort == VacancySortStates.VacancyIdAsc ? VacancySortStates.VacancyIdDesc : VacancySortStates.VacancyIdAsc;
            TitleSort = sort == VacancySortStates.TitleAsc ? VacancySortStates.TitleDesc : VacancySortStates.TitleAsc;
            SalarySort = sort == VacancySortStates.SalaryAsc ? VacancySortStates.SalaryDesc : VacancySortStates.SalaryAsc;
            CompanySort = sort == VacancySortStates.CompanyAsc ? VacancySortStates.CompanyDesc : VacancySortStates.CompanyAsc;
            DateSort = sort == VacancySortStates.DateAsc ? VacancySortStates.DateDesc : VacancySortStates.DateAsc;
            Current = sort;
        }
    }
}
