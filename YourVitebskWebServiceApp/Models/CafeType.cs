using System.ComponentModel.DataAnnotations;

namespace YourVitebskWebServiceApp.Models
{
    public class CafeType
    {
        [Key]
        public int? CafeTypeId { get; set; }

        [Required(ErrorMessage = "Необходимо указать тип зведения")]
        public string Name { get; set; }
    }
}
