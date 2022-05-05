using System.ComponentModel.DataAnnotations;

namespace YourVitebskWebServiceApp.Models
{
    public class User
    {
        [Key]
        public int? UserId { get; set; }

        [Required(ErrorMessage = "Необходимо ввести email")]
        [EmailAddress(ErrorMessage = "Некорректный адрес")]
        public string Email { get; set; }

        public string Password { get; set; }

        [Required(ErrorMessage = "Необходимо указать роль")]
        public int RoleId { get; set; }

        public UserDatum UserDatum { get; set; }
    }
}
