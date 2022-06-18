using System.ComponentModel.DataAnnotations;
using YourVitebskWebServiceApp.Interfaces;

namespace YourVitebskWebServiceApp.Models
{
    public class Role : IViewModel
    {
        [Key]
        public int? RoleId { get; set; }
        public string Name { get; set; }
    }
}
