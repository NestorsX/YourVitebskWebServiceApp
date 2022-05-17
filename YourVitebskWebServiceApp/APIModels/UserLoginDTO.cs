using System.ComponentModel.DataAnnotations;

namespace YourVitebskWebServiceApp.APIModels
{
    public class UserLoginDTO
    {
        [EmailAddress(ErrorMessage = "Некорректный адрес")]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
