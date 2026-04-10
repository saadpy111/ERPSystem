using Accounting.Application.Common.Models;
using Accounting.Application.Features.Vouchers.DTOs;
using Accounting.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;

namespace Accounting.Application.Features.Vouchers.Commands.CreateVoucher
{
    public class CreateVoucherCommand : IRequest<Result<int>>
    {
        public VoucherType VoucherType { get; set; }
        public DateTime Date { get; set; }
        public int? PartnerId { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public int CurrencyId { get; set; }
        public string? Reference { get; set; }
        public string? Description { get; set; }
        public List<CreateVoucherLineDto> Lines { get; set; } = new();
    }

    public class CreateVoucherLineDto
    {
        public int AccountId { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public int CurrencyId { get; set; }
        public int? CostCenterId { get; set; }
    }
}
