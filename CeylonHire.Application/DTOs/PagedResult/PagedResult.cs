namespace CeylonHire.Application.DTOs.PagedResult
{
    public class PagedResult<T>
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public IReadOnlyList<T> Items { get; set; }
    }
}
