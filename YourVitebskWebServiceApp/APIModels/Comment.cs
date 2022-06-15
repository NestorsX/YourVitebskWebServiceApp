namespace YourVitebskWebServiceApp.APIModels
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public string UserFirstName { get; set; }
        public string IsRecommend { get; set; }
        public string Message { get; set; }
        public string PublishDate { get; set; }
    }
}
