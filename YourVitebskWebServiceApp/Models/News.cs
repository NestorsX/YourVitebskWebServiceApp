using System.ComponentModel.DataAnnotations;
using YourVitebskWebServiceApp.Interfaces;

namespace YourVitebskWebServiceApp.Models
{
    public class News : IViewModel
    {
        [Key]
        public int? NewsId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ExternalLink { get; set; }
    }
}
