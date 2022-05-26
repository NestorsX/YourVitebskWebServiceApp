using System.ComponentModel.DataAnnotations;
using YourVitebskWebServiceApp.Interfaces;

namespace YourVitebskWebServiceApp.ViewModels
{
    public class UserViewModel : IViewModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Необходимо указать email")]
        [EmailAddress(ErrorMessage = "Некорректный адрес")]
        public string Email { get; set; }

        public string Password { get; set; }

        [Required(ErrorMessage = "Необходимо указать роль")]
        public int RoleId { get; set; }

        public string Role { get; set; }

        [Required(ErrorMessage = "Необходимо указать имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Необходимо указать фамилию")]
        public string LastName { get; set; }

        [Phone(ErrorMessage = "Неверный формат телефона")]
        public string PhoneNumber { get; set; }

        public bool IsVisible { get; set; }
    }
}
