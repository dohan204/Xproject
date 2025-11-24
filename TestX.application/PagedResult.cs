namespace TestX.application
{
    public class PagedResult<T>
    { 
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount/(double)PageSize);
        public List<T> Items { get; set; }
    }
}
