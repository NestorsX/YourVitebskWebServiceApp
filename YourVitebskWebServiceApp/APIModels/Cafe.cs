using YourVitebskWebServiceApp.Interfaces;

namespace YourVitebskWebServiceApp.APIModels
{
    public class Cafe
    {
        public int? CafeId { get; set; }
        public string CafeType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string WorkingTime { get; set; }
        public string Address { get; set; }
        public double? Rating { get; set; }
        public string ExternalLink { get; set; }
    }
}
