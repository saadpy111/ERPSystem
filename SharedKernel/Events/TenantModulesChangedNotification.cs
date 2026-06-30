using MediatR;

namespace SharedKernel.Events
{
    public class TenantModulesChangedNotification : INotification
    {
        public string TenantId { get; set; } = string.Empty;
    }
}
