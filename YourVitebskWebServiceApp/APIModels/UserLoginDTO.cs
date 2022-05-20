using System.ComponentModel.DataAnnotations;

namespace YourVitebskWebServiceApp.APIModels
{
    public class UserLoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
