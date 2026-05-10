using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Common.Models;
using Accounting.Application.Features.Vouchers.DTOs;
using Accounting.Application.Interfaces.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Accounting.Application.Features.Vouchers.Queries.GetVoucherById
{
    public class GetVoucherByIdQuery : IRequest<Result<VoucherDto>>
    {
        public int Id { get; set; }
    }

    public class GetVoucherByIdQueryHandler : IRequestHandler<GetVoucherByIdQuery, Result<VoucherDto>>
    {
        private readonly IAccountingDbContext _context;

        public GetVoucherByIdQueryHandler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<Result<VoucherDto>> Handle(GetVoucherByIdQuery request, CancellationToken cancellationToken)
        {
            var dto = await _context.Vouchers
                .AsNoTracking()
                .Where(v => v.Id == request.Id)
                .Select(v => new VoucherDto
                {
                    Id = v.Id,
                    VoucherNumber = v.VoucherNumber,
                    VoucherType = v.VoucherType,
                    Date = v.Date,
                    Status = v.Status,
                    PartnerId = v.PartnerId,
                    PaymentMethod = v.PaymentMethod,
                    CurrencyId = v.CurrencyId,
                    TotalAmount = v.TotalAmount,
                    Reference = v.Reference,
                    Description = v.Description,
                    JournalEntryId = v.JournalEntryId,
                    CreatedAt = v.CreatedAt,
                    CreatedBy = v.CreatedBy,
                    UpdatedAt = v.UpdatedAt,
                    UpdatedBy = v.UpdatedBy,

                    Lines = v.Lines.Select(l => new VoucherLineDto
                    {
                        Id = l.Id,
                        AccountId = l.AccountId,
                        Debit = l.Debit,
                        Credit = l.Credit,
                        CurrencyId = l.CurrencyId
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (dto == null)
            {
                return Result<VoucherDto>.Failure("Voucher not found");
            }

            return dto;
        }
    }
}