using System;

namespace YourVitebskWebServiceApp.APIModels
{
    public class CommentDTO
    {
        public int UserId { get; set; }
        public int ServiceId { get; set; }
        public int ItemId { get; set; }
        public bool IsRecommend { get; set; }
        public string Message { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
