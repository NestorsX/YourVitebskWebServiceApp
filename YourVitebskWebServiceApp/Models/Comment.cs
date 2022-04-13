using System.ComponentModel.DataAnnotations;

namespace YourVitebskWebServiceApp.Models
{
    public class Comment
    {
        [Key]
        public int? CommentId { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [Required]
        public int UserDataId { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
