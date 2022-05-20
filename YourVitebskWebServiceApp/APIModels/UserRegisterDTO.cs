using System.ComponentModel.DataAnnotations;

namespace YourVitebskWebServiceApp.APIModels
{
    public class UserRegisterDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
