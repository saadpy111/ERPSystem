using MediatR;
using Microsoft.EntityFrameworkCore;
using Accounting.Application.Common.Models;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Application.Features.Receivables.DTOs;
using Accounting.Domain.Entities;
using System.Linq;

namespace Accounting.Application.Features.Receivables.Queries.GetReceivablesPaged
{
    public class GetReceivablesPagedQueryHandler : IRequestHandler<GetReceivablesPagedQuery, GetReceivablesPagedResponse>
    {
        private readonly IUnitOfWork _uow;

        public GetReceivablesPagedQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<GetReceivablesPagedResponse> Handle(GetReceivablesPagedQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Receivable> query = _uow.Receivables.Query()
                .Include(r => r.Partner);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.ToLower();
                query = query.Where(r =>
                    r.Reference!.ToLower().Contains(search) ||
                    r.Description!.ToLower().Contains(search) ||
                    r.Partner.NameEn.ToLower().Contains(search) ||
                    r.Partner.NameAr.ToLower().Contains(search));
            }

            if (request.Status.HasValue)
                query = query.Where(r => r.Status == request.Status.Value);

            if (request.PartnerId.HasValue)
                query = query.Where(r => r.PartnerId == request.PartnerId.Value);

            if (request.DueDateFrom.HasValue)
                query = query.Where(r => r.DueDate >= request.DueDateFrom.Value);

            if (request.DueDateTo.HasValue)
                query = query.Where(r => r.DueDate <= request.DueDateTo.Value);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(r => r.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(r => new ReceivableListDto
                {
                    Id = r.Id,
                    PartnerId = r.PartnerId,
                    PartnerName = r.Partner.NameEn,
                    Amount = r.Amount,
                    PaidAmount = r.PaidAmount,
                    RemainingAmount = r.RemainingAmount,
                    DueDate = r.DueDate,
                    Status = r.Status,
                    Reference = r.Reference,
                    Description = r.Description,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return new GetReceivablesPagedResponse
            {
                Result = new PagedResult<ReceivableListDto>
                {
                    Data = items,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                }
            };
        }
    }
}
