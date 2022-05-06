using System.ComponentModel.DataAnnotations;
using YourVitebskWebServiceApp.Interfaces;

namespace YourVitebskWebServiceApp.Models
{
    public class CafeType : IViewModel
    {
        [Key]
        public int? CafeTypeId { get; set; }

        [Required(ErrorMessage = "Необходимо указать тип зведения")]
        public string Name { get; set; }
    }
}
