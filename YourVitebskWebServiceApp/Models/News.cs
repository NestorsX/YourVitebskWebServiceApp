using System.ComponentModel.DataAnnotations;
using YourVitebskWebServiceApp.Interfaces;

namespace YourVitebskWebServiceApp.Models
{
    public class News : IViewModel
    {
        public int? NewsId { get; set; }

        [Required(ErrorMessage = "Необходимо указать заголовок")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Необходимо указать описание")]
        public string Description { get; set; }

        public string ExternalLink { get; set; }
    }
}
