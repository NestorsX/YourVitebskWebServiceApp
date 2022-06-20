namespace YourVitebskWebServiceApp.Helpers.Filterers
{
    public class VacancyFilterer
    {
        public VacancyFilterer(string searchLine)
        {
            SearchLine = searchLine;
        }

        public string SearchLine { get; private set; }
    }
}
