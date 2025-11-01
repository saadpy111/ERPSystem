using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using Hr.Application.Pagination;
using MediatR;

namespace Hr.Application.Features.LoanInstallmentFeatures.GetLoanInstallmentsPaged
{
    public class GetLoanInstallmentsPagedHandler : IRequestHandler<GetLoanInstallmentsPagedRequest, GetLoanInstallmentsPagedResponse>
    {
        private readonly ILoanInstallmentRepository _repository;
        private readonly IMapper _mapper;

        public GetLoanInstallmentsPagedHandler(ILoanInstallmentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetLoanInstallmentsPagedResponse> Handle(GetLoanInstallmentsPagedRequest request, CancellationToken cancellationToken)
        {
            var query = (await _repository.GetAllAsync()).AsQueryable();

            // Apply loan filter
            if (request.LoanId.HasValue)
            {
                query = query.Where(li => li.LoanId == request.LoanId.Value);
            }

            // Apply status filter
            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                if (Enum.TryParse<Hr.Domain.Enums.InstallmentStatus>(request.Status, true, out var status))
                {
                    query = query.Where(li => li.Status == status);
                }
            }

            var totalCount = query.Count();

            // Apply ordering
            query = ApplyOrdering(query, request.OrderBy, request.IsDescending);

            // Apply pagination
            var items = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var dtos = _mapper.Map<IEnumerable<LoanInstallmentDto>>(items);

            return new GetLoanInstallmentsPagedResponse
            {
                PagedResult = new PagedResult<LoanInstallmentDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                }
            };
        }

        private IQueryable<Hr.Domain.Entities.LoanInstallment> ApplyOrdering(
            IQueryable<Hr.Domain.Entities.LoanInstallment> query,
            string? orderBy,
            bool isDescending)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "DueDate";

            query = orderBy.ToLower() switch
            {
                "duedate" => isDescending ? query.OrderByDescending(li => li.DueDate) : query.OrderBy(li => li.DueDate),
                "amountdue" => isDescending ? query.OrderByDescending(li => li.AmountDue) : query.OrderBy(li => li.AmountDue),
                "paymentdate" => isDescending ? query.OrderByDescending(li => li.PaymentDate) : query.OrderBy(li => li.PaymentDate),
                "status" => isDescending ? query.OrderByDescending(li => li.Status) : query.OrderBy(li => li.Status),
                _ => isDescending ? query.OrderByDescending(li => li.DueDate) : query.OrderBy(li => li.DueDate)
            };

            return query;
        }
    }
}
