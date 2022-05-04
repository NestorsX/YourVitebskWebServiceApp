using System.ComponentModel.DataAnnotations;

namespace YourVitebskWebServiceApp.Models
{
    public class PosterType
    {
        [Key]
        public int? PosterTypeId { get; set; }

        [Required(ErrorMessage = "Необходимо ввести тип искусства")]
        public string Name { get; set; }
    }
}
