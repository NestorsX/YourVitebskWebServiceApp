using System.ComponentModel.DataAnnotations;

namespace YourVitebskWebServiceApp.Models
{
    public class User
    {
        [Key]
        public int? UserId { get; set; }

        [Required(ErrorMessage = "Необходимо указать email")]
        [EmailAddress(ErrorMessage = "Некорректный адрес")]
        public string Email { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        [Required(ErrorMessage = "Необходимо указать роль")]
        public int RoleId { get; set; }

        public bool IsVisible { get; set; }

        public UserDatum UserDatum { get; set; }
    }
}
