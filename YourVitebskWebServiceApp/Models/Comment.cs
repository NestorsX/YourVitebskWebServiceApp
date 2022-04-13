namespace YourVitebskWebServiceApp
{
    public class Comment
    {
        public int? CommentId { get; set; }
        public int ServiceId { get; set; }
        public int UserDataId { get; set; }
        public string Message { get; set; }
    }
}
