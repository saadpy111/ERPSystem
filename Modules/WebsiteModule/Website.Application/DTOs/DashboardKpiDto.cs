namespace Website.Application.DTOs
{
    public class DashboardKpiDto
    {
        public decimal PaymentSuccessRate { get; set; }
        public int PaidOrders { get; set; }
        public decimal NetRevenue { get; set; }
        public decimal GrossRevenue { get; set; }
        public decimal AverageOrderValue { get; set; }
        public decimal GrowthRate { get; set; }
        public decimal ReturnRate { get; set; }
        public decimal TotalDiscounts { get; set; }
    }
}
