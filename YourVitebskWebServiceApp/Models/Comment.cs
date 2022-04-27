using System.ComponentModel.DataAnnotations;

namespace YourVitebskWebServiceApp.Models
{
    public class Comment
    {
        [Key]
        public int? CommentId { get; set; }

        public int ServiceId { get; set; }

        public int UserDataId { get; set; }

        public string Message { get; set; }
    }
}
