namespace Website.Application.Features.OrderFeatures.Commands.CreateOrder
{
    public class CreateOrderCommandResponse
    {
        public bool Success { get; set; }
        public Guid? OrderId { get; set; }
        public string? OrderNumber { get; set; }
        public string? Message { get; set; }
    }
}
