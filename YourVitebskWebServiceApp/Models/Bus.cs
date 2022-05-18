using System.ComponentModel.DataAnnotations;
using YourVitebskWebServiceApp.Interfaces;

namespace YourVitebskWebServiceApp.Models
{
    public class Bus : IViewModel
    {
        [Key]
        public int? BusId { get; set; }

        [Required(ErrorMessage = "Необходимо указать номер автобуса")]
        public string Number { get; set; }
    }
}
