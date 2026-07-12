using MediatR;

namespace Website.Application.Features.WebsiteInitialization.Queries.CheckDomainAvailability
{
    public class CheckDomainAvailabilityQuery : IRequest<CheckDomainAvailabilityResponse>
    {
        public string Domain { get; set; } = string.Empty;
    }

    public class CheckDomainAvailabilityResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public bool IsAvailable { get; set; }
    }
}
