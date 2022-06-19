namespace YourVitebskWebServiceApp.Helpers.Filterers
{
    public class RoleFilterer
    {
        public RoleFilterer(string searchLine)
        {
            SearchLine = searchLine;
        }

        public string SearchLine { get; private set; }
    }
}
