using MediatR;
using Accounting.Domain.Enums;

namespace Accounting.Application.Features.Payables.Queries.GetPayablesPaged
{
    public class GetPayablesPagedQuery : IRequest<GetPayablesPagedResponse>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? Search { get; set; }
        public PayableStatus? Status { get; set; }
        public int? PartnerId { get; set; }
        public DateTime? DueDateFrom { get; set; }
        public DateTime? DueDateTo { get; set; }
    }
}
