using System;
using System.Collections.Generic;
using Accounting.Domain.Common;
using Accounting.Domain.Enums;

namespace Accounting.Domain.Entities
{
    public class Receivable : BaseEntity
    {
        public int PartnerId { get; set; }
        public decimal Amount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public DateTime DueDate { get; set; }
        public ReceivableStatus Status { get; set; }
        public string? Reference { get; set; }
        public string? Description { get; set; }

        public virtual Partner Partner { get; set; } = null!;
        public virtual ICollection<ReceivablePayment> Payments { get; set; } = new List<ReceivablePayment>();
    }
}
