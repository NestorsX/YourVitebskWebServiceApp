using System.ComponentModel.DataAnnotations;

namespace YourVitebskWebServiceApp.Models
{
    public class User
    {
        [Key]
        public int? UserId { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public int RoleId { get; set; }
        public bool IsVisible { get; set; }
    }
}
