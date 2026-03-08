namespace Website.Application.DTOs
{
    public class WeeklyAnalyticsDto
    {
        public string DayOfWeek { get; set; } = string.Empty;
        public decimal Sales { get; set; }
        public int Orders { get; set; }
        public int Visitors { get; set; }
    }
}
