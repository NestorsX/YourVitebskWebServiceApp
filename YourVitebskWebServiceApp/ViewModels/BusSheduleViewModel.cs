using System.ComponentModel.DataAnnotations;
using YourVitebskWebServiceApp.Interfaces;

namespace YourVitebskWebServiceApp.ViewModels
{
    public class BusSheduleViewModel : IViewModel
    {
        [Key]
        public int? BusSheduleId { get; set; }
        public string BusNumber { get; set; }
        public string BusStopName { get; set; }
        public int BusRoute { get; set; }
        public int BusStopNumber { get; set; }
        public string WorkDayShedule { get; set; }
        public string DayOffShedule { get; set; }
    }
}
