using System.Collections.Generic;
using YourVitebskWebServiceApp.Interfaces;

namespace YourVitebskWebServiceApp.APIModels
{
    public class Poster
    {
        public int? PosterId { get; set; }
        public string PosterType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DateTime { get; set; }
        public string Address { get; set; }
        public string ExternalLink { get; set; }
        public string TitleImage { get; set; }
        public IEnumerable<string> Images { get; set; }
    }
}
