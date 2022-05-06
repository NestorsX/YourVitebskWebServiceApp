using System.ComponentModel.DataAnnotations;

namespace YourVitebskWebServiceApp.Models
{
    public class UserDatum
    {
        [Key]
        public int? UserDataId { get; set; }

        public int? UserId { get; set; }

        [Required(ErrorMessage = "Необходимо ввести имя")]
        public string FirstName { get; set; }

        public string SecondName { get; set; }

        [Required(ErrorMessage = "Необходимо ввести фамилию")]
        public string LastName { get; set; }

        [Phone(ErrorMessage = "Неверный формат телефона")]
        public string PhoneNumber { get; set; }
    }
}
