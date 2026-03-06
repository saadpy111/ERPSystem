namespace Website.Application.DTOs
{
    public class CustomerFilter
    {
        /// <summary>
        /// Searches by FullName or Email (matched against Identity user data via join).
        /// </summary>
        public string? Search { get; set; }

        public DateTime? JoinedFrom { get; set; }
        public DateTime? JoinedTo { get; set; }

        /// <summary>
        /// Filter by minimum number of orders placed.
        /// </summary>
        public int? MinOrdersCount { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
