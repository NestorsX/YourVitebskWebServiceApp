using System.ComponentModel.DataAnnotations;

namespace YourVitebskWebServiceApp.Models
{
    public class Service
    {
        [Key]
        public int? ServiceId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
