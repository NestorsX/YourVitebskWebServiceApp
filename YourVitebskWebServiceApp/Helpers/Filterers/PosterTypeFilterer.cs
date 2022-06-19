namespace YourVitebskWebServiceApp.Helpers.Filterers
{
    public class PosterTypeFilterer
    {
        public PosterTypeFilterer(string searchLine)
        {
            SearchLine = searchLine;
        }

        public string SearchLine { get; private set; }
    }
}
