using System.ComponentModel.DataAnnotations;
using YourVitebskWebServiceApp.Interfaces;

namespace YourVitebskWebServiceApp.Models
{
    public class PosterType : IViewModel
    {
        [Key]
        public int? PosterTypeId { get; set; }

        [Required(ErrorMessage = "Необходимо указать тип искусства")]
        public string Name { get; set; }
    }
}
