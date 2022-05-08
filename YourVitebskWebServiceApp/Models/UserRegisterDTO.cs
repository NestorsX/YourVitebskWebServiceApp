using System.ComponentModel.DataAnnotations;

namespace YourVitebskWebServiceApp.Models
{
    public class UserRegisterDTO
    {
        [EmailAddress(ErrorMessage = "Некорректный адрес")]
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
