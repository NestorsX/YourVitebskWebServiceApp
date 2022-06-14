namespace YourVitebskWebServiceApp.ViewModels
{
    public class CommentViewModel
    {
        public int CommentId { get; set; }
        public string UserEmail { get; set; }
        public string Service { get; set; }
        public string ItemName { get; set; }
        public string IsRecommend { get; set; }
        public string Message { get; set; }
        public string PublishDate { get; set; }
    }
}
