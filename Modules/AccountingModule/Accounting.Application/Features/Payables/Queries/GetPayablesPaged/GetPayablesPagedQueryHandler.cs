using MediatR;
using Microsoft.EntityFrameworkCore;
using Accounting.Application.Common.Models;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Application.Features.Payables.DTOs;
using Accounting.Domain.Entities;
using System.Linq;

namespace Accounting.Application.Features.Payables.Queries.GetPayablesPaged
{
    public class GetPayablesPagedQueryHandler : IRequestHandler<GetPayablesPagedQuery, GetPayablesPagedResponse>
    {
        private readonly IUnitOfWork _uow;

        public GetPayablesPagedQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<GetPayablesPagedResponse> Handle(GetPayablesPagedQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Payable> query = _uow.Payables.Query()
                .Include(p => p.Partner);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.ToLower();
                query = query.Where(p =>
                    p.Reference!.ToLower().Contains(search) ||
                    p.Description!.ToLower().Contains(search) ||
                    p.Partner.NameEn.ToLower().Contains(search) ||
                    p.Partner.NameAr.ToLower().Contains(search));
            }

            if (request.Status.HasValue)
                query = query.Where(p => p.Status == request.Status.Value);

            if (request.PartnerId.HasValue)
                query = query.Where(p => p.PartnerId == request.PartnerId.Value);

            if (request.DueDateFrom.HasValue)
                query = query.Where(p => p.DueDate >= request.DueDateFrom.Value);

            if (request.DueDateTo.HasValue)
                query = query.Where(p => p.DueDate <= request.DueDateTo.Value);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new PayableListDto
                {
                    Id = p.Id,
                    PartnerId = p.PartnerId,
                    PartnerName = p.Partner.NameEn,
                    Amount = p.Amount,
                    PaidAmount = p.PaidAmount,
                    RemainingAmount = p.RemainingAmount,
                    DueDate = p.DueDate,
                    Status = p.Status,
                    Reference = p.Reference,
                    Description = p.Description,
                    CreatedAt = p.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return new GetPayablesPagedResponse
            {
                Result = new PagedResult<PayableListDto>
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
