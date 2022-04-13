using System.ComponentModel.DataAnnotations;

namespace YourVitebskWebServiceApp.Models
{
    public class UserDatum
    {
        [Key]
        public int? UserDataId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string SecondName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }
    }
}
