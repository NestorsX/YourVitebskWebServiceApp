using System;
using System.ComponentModel.DataAnnotations;

namespace YourVitebskWebServiceApp.Models
{
    public class Comment
    {
        [Key]
        public int? CommentId { get; set; }
        public int UserId { get; set; }
        public int ServiceId { get; set; }
        public int ItemId { get; set; }
        public bool IsRecommend { get; set; }
        public string Message { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
