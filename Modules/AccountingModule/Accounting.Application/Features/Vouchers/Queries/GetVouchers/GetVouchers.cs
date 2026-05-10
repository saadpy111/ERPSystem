using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Common.Models;
using Accounting.Application.Interfaces.Contexts;
using Accounting.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Accounting.Application.Features.Vouchers.Queries.GetVouchers
{
    public class VoucherListDto
    {
        public int Id { get; set; }
        public string VoucherNumber { get; set; } = null!;
        public VoucherType VoucherType { get; set; }
        public VoucherStatus Status { get; set; }
        public DateTime Date { get; set; }
        public string? PartnerName { get; set; }
        public string CurrencyCode { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public int? JournalEntryId { get; set; }
    }

    public class GetVouchersQuery : IRequest<Result<PagedResult<VoucherListDto>>>
    {
        public VoucherType? VoucherType { get; set; }
        public VoucherStatus? Status { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string? VoucherNumber { get; set; }
        public int? PartnerId { get; set; }
        public int? CurrencyId { get; set; }
        public string? Search { get; set; }
        public bool? PostedOnly { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetVouchersQueryHandler : IRequestHandler<GetVouchersQuery, Result<PagedResult<VoucherListDto>>>
    {
        private readonly IAccountingDbContext _context;

        public GetVouchersQueryHandler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<Result<PagedResult<VoucherListDto>>> Handle(GetVouchersQuery request, CancellationToken cancellationToken)
        {
            var q = _context.Vouchers.AsNoTracking();

            if (request.VoucherType.HasValue)
                q = q.Where(v => v.VoucherType == request.VoucherType.Value);

            if (request.Status.HasValue)
                q = q.Where(v => v.Status == request.Status.Value);

            if (request.DateFrom.HasValue)
                q = q.Where(v => v.Date >= request.DateFrom.Value);

            if (request.DateTo.HasValue)
                q = q.Where(v => v.Date <= request.DateTo.Value);

            if (!string.IsNullOrWhiteSpace(request.VoucherNumber))
                q = q.Where(v => v.VoucherNumber.Contains(request.VoucherNumber));

            if (request.PartnerId.HasValue)
                q = q.Where(v => v.PartnerId == request.PartnerId.Value);

            if (request.CurrencyId.HasValue)
                q = q.Where(v => v.CurrencyId == request.CurrencyId.Value);

            if (request.PostedOnly.HasValue && request.PostedOnly.Value)
                q = q.Where(v => v.Status == VoucherStatus.Posted);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.ToLower();
                q = q.Where(v => v.VoucherNumber.ToLower().Contains(search) || 
                                (v.Description != null && v.Description.ToLower().Contains(search)) ||
                                (v.Reference != null && v.Reference.ToLower().Contains(search)));
            }

            var totalCount = await q.CountAsync(cancellationToken);

            var items = await q
                .OrderByDescending(v => v.Date)
                .ThenByDescending(v => v.VoucherNumber)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(v => new VoucherListDto
                {
                    Id = v.Id,
                    VoucherNumber = v.VoucherNumber,
                    VoucherType = v.VoucherType,
                    Status = v.Status,
                    Date = v.Date,
                    PartnerName = v.Partner != null ? v.Partner.NameEn : null,
                    CurrencyCode = v.Currency.Code,
                    TotalAmount = v.TotalAmount,
                    JournalEntryId = v.JournalEntryId
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<VoucherListDto>
            {
                Data = items,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
}
