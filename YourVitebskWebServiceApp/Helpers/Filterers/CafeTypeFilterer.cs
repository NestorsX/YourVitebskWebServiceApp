namespace YourVitebskWebServiceApp.Helpers.Filterers
{
    public class CafeTypeFilterer
    {
        public CafeTypeFilterer(string searchLine)
        {
            SearchLine = searchLine;
        }

        public string SearchLine { get; private set; }
    }
}
