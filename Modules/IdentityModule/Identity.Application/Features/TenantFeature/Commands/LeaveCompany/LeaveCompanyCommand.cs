using MediatR;

namespace Identity.Application.Features.TenantFeature.Commands.LeaveCompany
{
    public class LeaveCompanyCommand : IRequest<LeaveCompanyResponse>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
