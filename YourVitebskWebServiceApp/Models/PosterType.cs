using System.ComponentModel.DataAnnotations;

namespace YourVitebskWebServiceApp.Models
{
    public class PosterType
    {
        [Key]
        public int? PosterTypeId { get; set; }
        public string Name { get; set; }
    }
}
