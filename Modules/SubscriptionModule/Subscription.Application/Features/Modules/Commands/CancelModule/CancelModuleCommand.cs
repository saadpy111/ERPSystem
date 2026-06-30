using MediatR;

namespace Subscription.Application.Features.Modules.Commands.CancelModule
{
    public class CancelModuleCommand : IRequest<CancelModuleResponse>
    {
        public string TenantId { get; set; } = string.Empty;
        public string ModuleCode { get; set; } = string.Empty;
    }

    public class CancelModuleResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
    }
}
