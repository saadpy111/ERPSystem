using MediatR;
using Subscription.Application.DTOs;

namespace Subscription.Application.Features.Modules.Queries.GetAvailableModules
{
    public class GetAvailableModulesQuery : IRequest<GetAvailableModulesResponse>
    {
        public string? CurrencyCode { get; set; }
    }

    public class GetAvailableModulesResponse
    {
        public bool Success { get; set; }
        public List<ModuleDto> Data { get; set; } = new();
    }
}
