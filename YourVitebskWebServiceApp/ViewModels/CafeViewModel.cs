using System.ComponentModel.DataAnnotations;
using YourVitebskWebServiceApp.Interfaces;

namespace YourVitebskWebServiceApp.ViewModels
{
    public class CafeViewModel : IViewModel
    {
        [Key]
        public int? CafeId { get; set; }
        public int CafeTypeId { get; set; }
        public string CafeType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string WorkingTime { get; set; }
        public string Address { get; set; }
        public string ExternalLink { get; set; }
    }
}
