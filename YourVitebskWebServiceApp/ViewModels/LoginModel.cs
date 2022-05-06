using System.ComponentModel.DataAnnotations;

namespace YourVitebskWebServiceApp.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Необходимо указать Email")]
        [EmailAddress(ErrorMessage = "Некорректный адрес")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Необходимо указать пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
