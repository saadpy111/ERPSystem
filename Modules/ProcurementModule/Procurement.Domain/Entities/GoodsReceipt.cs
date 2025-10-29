using System;
using System.Collections.Generic;

namespace Procurement.Domain.Entities
{
    public class GoodsReceipt
    {
        public Guid Id { get; private set; }
        public Guid WarehouseId { get;  set; }
        public Guid PurchaseOrderId { get;  set; }
        public PurchaseOrder  PurchaseOrder { get; set; }
        public Guid LocationId { get;  set; }
        public bool IsConfirmed { get; private set; }
        public List<GoodsReceiptItem> Items { get; private set; } = new();

        public string? Remarks { get; set; }

        public Guid? ReceivedBy { get; set; }
        public DateTime ReceivedDate { get; set; }

        
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; private set; }

        public GoodsReceipt(Guid warehouseId)
        {
            Id = Guid.NewGuid();
            WarehouseId = warehouseId;
            IsConfirmed = false;
        }

        public void AddItem(Guid productId, decimal quantity)
        {
            Items.Add(new GoodsReceiptItem
            {
                ProductId = productId,
                 ReceivedQuantity = (int)quantity
            });
        }

        public void ConfirmReceipt()
        {
            if (IsConfirmed)
                return;

            IsConfirmed = true;
            UpdatedAt = DateTime.UtcNow;
        }
    }

    //public class GoodsReceiptItem
    //{
    //    public Guid ProductId { get; set; }
    //    public decimal Quantity { get; set; }
    //}
}
