using Accounting.Application.Common.Models;
using Accounting.Application.Features.Payables.DTOs;

namespace Accounting.Application.Features.Payables.Queries.GetPayablesPaged
{
    public class GetPayablesPagedResponse
    {
        public PagedResult<PayableListDto> Result { get; set; } = null!;
    }
}
