namespace Website.Application.Pagination
{
    public class CouponFilter
    {
        public string? SearchTerm { get; set; } // Matches Name or Code
        public bool? IsActive { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; } // "asc" or "desc"
    }
}
