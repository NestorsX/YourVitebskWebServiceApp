using System.ComponentModel.DataAnnotations;
using YourVitebskWebServiceApp.Interfaces;

namespace YourVitebskWebServiceApp.ViewModels
{
    public class BusSheduleViewModel : IViewModel
    {
        [Key]
        public int? BusSheduleId { get; set; }

        [Required(ErrorMessage = "Необходимо указать номер автобуса")]
        public int BusId { get; set; }

        public string BusNumber { get; set; }

        [Required(ErrorMessage = "Необходимо указать остановку")]
        public int BusStopId { get; set; }

        public string BusStopName { get; set; }

        [Required(ErrorMessage = "Необходимо указать номер маршрута")]
        public int BusRoute { get; set; }

        [Required(ErrorMessage = "Необходимо указать номер остановки в маршруте")]
        public int BusStopNumber { get; set; }

        [Required(ErrorMessage = "Необходимо указать рабочий или выходной день")]
        public bool IsWorkday { get; set; }

        [Required(ErrorMessage = "Необходимо указать расписание")]
        public string Time { get; set; }
    }
}
