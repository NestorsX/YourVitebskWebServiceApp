using System.ComponentModel.DataAnnotations;

namespace YourVitebskWebServiceApp.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Необходимо ввести Email")]
        [EmailAddress(ErrorMessage = "Некорректный адрес")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Необходимо ввести пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
