using System.ComponentModel.DataAnnotations;

namespace YourVitebskWebServiceApp.Models
{
    public class BusShedule
    {
        [Key]
        public int? BusSheduleId { get; set; }

        [Required(ErrorMessage = "Необходимо указать номер автобуса")]
        public int BusId { get; set; }

        [Required(ErrorMessage = "Необходимо указать остановку")]
        public int BusStopId { get; set; }

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
