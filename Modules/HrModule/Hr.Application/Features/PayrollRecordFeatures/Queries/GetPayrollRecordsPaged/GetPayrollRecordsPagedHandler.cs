using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using Hr.Application.Pagination;
using MediatR;

namespace Hr.Application.Features.PayrollRecordFeatures.GetPayrollRecordsPaged
{
    public class GetPayrollRecordsPagedHandler : IRequestHandler<GetPayrollRecordsPagedRequest, GetPayrollRecordsPagedResponse>
    {
        private readonly IPayrollRecordRepository _repository;
        private readonly IMapper _mapper;

        public GetPayrollRecordsPagedHandler(IPayrollRecordRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetPayrollRecordsPagedResponse> Handle(GetPayrollRecordsPagedRequest request, CancellationToken cancellationToken)
        {
            var query = (await _repository.GetAllAsync()).AsQueryable();

            // Apply employee filter
            if (request.EmployeeId.HasValue)
            {
                query = query.Where(pr => pr.EmployeeId == request.EmployeeId.Value);
            }

            // Apply period year filter
            if (request.PeriodYear.HasValue)
            {
                query = query.Where(pr => pr.PeriodYear == request.PeriodYear.Value);
            }

            // Apply period month filter
            if (request.PeriodMonth.HasValue)
            {
                query = query.Where(pr => pr.PeriodMonth == request.PeriodMonth.Value);
            }

            // Apply status filter
            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                if (Enum.TryParse<Hr.Domain.Enums.PayrollStatus>(request.Status, true, out var status))
                {
                    query = query.Where(pr => pr.Status == status);
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

            var dtos = _mapper.Map<IEnumerable<PayrollRecordDto>>(items);

            return new GetPayrollRecordsPagedResponse
            {
                PagedResult = new PagedResult<PayrollRecordDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                }
            };
        }

        private IQueryable<Hr.Domain.Entities.PayrollRecord> ApplyOrdering(
            IQueryable<Hr.Domain.Entities.PayrollRecord> query,
            string? orderBy,
            bool isDescending)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "PeriodYear";

            query = orderBy.ToLower() switch
            {
                "periodyear" => isDescending ? query.OrderByDescending(pr => pr.PeriodYear) : query.OrderBy(pr => pr.PeriodYear),
                "periodmonth" => isDescending ? query.OrderByDescending(pr => pr.PeriodMonth) : query.OrderBy(pr => pr.PeriodMonth),
                "netsalary" => isDescending ? query.OrderByDescending(pr => pr.NetSalary) : query.OrderBy(pr => pr.NetSalary),
                "status" => isDescending ? query.OrderByDescending(pr => pr.Status) : query.OrderBy(pr => pr.Status),
                _ => isDescending ? query.OrderByDescending(pr => pr.PeriodYear) : query.OrderBy(pr => pr.PeriodYear)
            };

            return query;
        }
    }
}
