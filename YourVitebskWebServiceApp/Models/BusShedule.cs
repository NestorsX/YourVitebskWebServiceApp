using System.ComponentModel.DataAnnotations;

namespace YourVitebskWebServiceApp.Models
{
    public class BusShedule
    {
        [Key]
        public int? BusSheduleId { get; set; }
        public int BusId { get; set; }
        public int BusStopId { get; set; }
        public int BusRoute { get; set; }

        [Range(1,100, ErrorMessage = "Нумерация остановок начинается с 1")]
        public int BusStopNumber { get; set; }
        public bool IsWorkday { get; set; }
        public string Time { get; set; }
    }
}
