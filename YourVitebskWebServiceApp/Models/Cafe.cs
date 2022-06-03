using System.ComponentModel.DataAnnotations;
using YourVitebskWebServiceApp.Interfaces;

namespace YourVitebskWebServiceApp.Models
{
    public class Cafe : IViewModel
    {
        [Key]
        public int? CafeId { get; set; }

        [Required(ErrorMessage = "Необходимо указать тип заведения")]
        public int CafeTypeId { get; set; }

        [Required(ErrorMessage = "Необходимо указать название заведения")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Необходимо указать описание")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Необходимо указать время работы")]
        public string WorkingTime { get; set; }

        [Required(ErrorMessage = "Необходимо указать адрес")]
        public string Address { get; set; }

        public string ExternalLink { get; set; }
    }
}
