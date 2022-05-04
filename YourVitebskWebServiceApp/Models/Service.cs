using System.ComponentModel.DataAnnotations;
using YourVitebskWebServiceApp.Interfaces;

namespace YourVitebskWebServiceApp.Models
{
    public class Service : IViewModel
    {
        [Key]
        public int? ServiceId { get; set; }

        [Required(ErrorMessage = "Необходимо ввести название сервиса")]
        public string Name { get; set; }
    }
}
