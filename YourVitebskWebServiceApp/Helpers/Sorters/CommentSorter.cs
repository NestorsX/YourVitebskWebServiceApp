using YourVitebskWebServiceApp.Helpers.SortStates;

namespace YourVitebskWebServiceApp.Helpers.Sorters
{
    public class CommentSorter
    {
        public CommentSortStates IdSort { get; set; }
        public CommentSortStates UserSort { get; set; }
        public CommentSortStates ServiceSort { get; set; }
        public CommentSortStates ItemSort { get; set; }
        public CommentSortStates IsRecommendSort { get; set; }
        public CommentSortStates DateSort { get; set; }
        public CommentSortStates Current { get; set; }

        public CommentSorter(CommentSortStates sort)
        {
            IdSort = sort == CommentSortStates.CommentIdAsc ? CommentSortStates.CommentIdDesc : CommentSortStates.CommentIdAsc;
            UserSort = sort == CommentSortStates.UserAsc ? CommentSortStates.UserDesc : CommentSortStates.UserAsc;
            ServiceSort = sort == CommentSortStates.ServiceAsc ? CommentSortStates.ServiceDesc : CommentSortStates.ServiceAsc;
            ItemSort = sort == CommentSortStates.ItemAsc ? CommentSortStates.ItemDesc : CommentSortStates.ItemAsc;
            IsRecommendSort = sort == CommentSortStates.IsRecommendAsc ? CommentSortStates.IsRecommendDesc : CommentSortStates.IsRecommendAsc;
            DateSort = sort == CommentSortStates.PublishDateAsc ? CommentSortStates.PublishDateDesc : CommentSortStates.PublishDateAsc;
            Current = sort;
        }
    }
}
