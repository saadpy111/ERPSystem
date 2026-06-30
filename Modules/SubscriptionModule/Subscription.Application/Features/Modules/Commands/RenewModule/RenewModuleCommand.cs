using MediatR;

namespace Subscription.Application.Features.Modules.Commands.RenewModule
{
    public class RenewModuleCommand : IRequest<RenewModuleResponse>
    {
        public string TenantId { get; set; } = string.Empty;
        public string ModuleCode { get; set; } = string.Empty;
    }

    public class RenewModuleResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
    }
}
