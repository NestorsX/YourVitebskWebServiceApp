using System.ComponentModel.DataAnnotations;

namespace YourVitebskWebServiceApp.Models
{
    public class CafeType
    {
        [Key]
        public int? CafeTypeId { get; set; }
        public string Name { get; set; }
    }
}
