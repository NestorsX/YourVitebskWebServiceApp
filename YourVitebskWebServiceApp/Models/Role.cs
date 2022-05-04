using System.ComponentModel.DataAnnotations;
using YourVitebskWebServiceApp.Interfaces;

namespace YourVitebskWebServiceApp.Models
{
    public class Role : IViewModel
    {
        [Key]
        public int? RoleId { get; set; }

        [Required(ErrorMessage = "Необходимо ввести название роли")]
        public string Name { get; set; }
    }
}
