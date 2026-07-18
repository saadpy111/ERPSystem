using Accounting.Application.Common.Models;
using Accounting.Application.Features.Receivables.DTOs;

namespace Accounting.Application.Features.Receivables.Queries.GetReceivablesPaged
{
    public class GetReceivablesPagedResponse
    {
        public PagedResult<ReceivableListDto> Result { get; set; } = null!;
    }
}
