using System.ComponentModel.DataAnnotations;

namespace YourVitebskWebServiceApp.Models
{
    public class Role
    {
        [Key]
        public int? RoleId { get; set; }
        public string Name { get; set; }
    }
}
