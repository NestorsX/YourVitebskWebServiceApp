using System.ComponentModel.DataAnnotations;
using YourVitebskWebServiceApp.Interfaces;

namespace YourVitebskWebServiceApp.Models
{
    public class BusStop : IViewModel
    {
        [Key]
        public int? BusStopId { get; set; }

        [Required(ErrorMessage = "Необходимо указать название остановки")]
        public string Name { get; set; }
    }
}
