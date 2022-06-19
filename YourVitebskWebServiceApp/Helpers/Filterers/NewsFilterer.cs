namespace YourVitebskWebServiceApp.Helpers.Filterers
{
    public class NewsFilterer
    {
        public NewsFilterer(string searchLine)
        {
            SearchLine = searchLine;
        }

        public string SearchLine { get; private set; }
    }
}
