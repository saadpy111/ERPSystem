using MediatR;
using Subscription.Application.DTOs;

namespace Subscription.Application.Features.Plans.Queries.GetAvailablePlans
{
    public class GetAvailablePlansQuery : IRequest<GetAvailablePlansResponse>
    {
        public string? CurrencyCode { get; set; } // Optional filter
        public bool IncludeHidden { get; set; } = false;
    }

    public class GetAvailablePlansResponse
    {
        public bool Success { get; set; }
        public List<AvailablePlanDto> Plans { get; set; } = new();
    }
}
