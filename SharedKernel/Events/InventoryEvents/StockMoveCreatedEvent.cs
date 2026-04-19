using MediatR;
using System;

namespace Events.InventoryEvents
{
    public class StockMoveCreatedEvent : INotification
    {
        public Guid StockMoveId { get; set; }
        public Guid ProductId { get; set; }
        public decimal Quantity { get; set; }
        public string MoveType { get; set; }
        public string? Reference { get; set; }
        public Guid? SourceLocationId { get; set; }
        public Guid? DestinationLocationId { get; set; }
        public DateTime MoveDate { get; set; }
    }
}
