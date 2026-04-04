using Accounting.Domain.Enums;
using System;

namespace Accounting.Application.Reports.DTOs
{
    public class AccountStatementDto
    {
        public DateTime Date { get; set; }
        public string? Reference { get; set; }
        public string? Description { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal RunningBalance { get; set; }
        public SourceType? SourceType { get; set; }
    }
}
