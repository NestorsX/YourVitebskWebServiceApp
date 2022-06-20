using System.ComponentModel.DataAnnotations;
using YourVitebskWebServiceApp.Interfaces;

namespace YourVitebskWebServiceApp.Models
{
    public class CafeType : IViewModel
    {
        [Key]
        public int? CafeTypeId { get; set; }
        public string Name { get; set; }
    }
}
