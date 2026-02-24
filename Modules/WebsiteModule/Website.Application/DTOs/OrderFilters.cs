using Website.Domain.Enums;

namespace Website.Application.DTOs
{
    public class AdminOrderFilter
    {
        public OrderStatus? Status { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string? SearchTerm { get; set; } // OrderNumber
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
