using MediatR;
using SharedKernel.Enums;

namespace Identity.Application.Features.TenantFeature.Commands.CreateCompany
{
    public class CreateCompanyCommand : IRequest<CreateCompanyResponse>
    {
        public string UserId { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string CompanyCode { get; set; } = string.Empty;
        public string PlanCode { get; set; } = string.Empty;
        public string CurrencyCode { get; set; } = string.Empty;
        public BillingInterval Interval { get; set; } = BillingInterval.Monthly;
    }
}
