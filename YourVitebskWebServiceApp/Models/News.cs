using System.ComponentModel.DataAnnotations;

namespace YourVitebskWebServiceApp.Models
{
    public class News
    {
        [Key]
        public int? NewsId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ExternalLink { get; set; }
    }
}
