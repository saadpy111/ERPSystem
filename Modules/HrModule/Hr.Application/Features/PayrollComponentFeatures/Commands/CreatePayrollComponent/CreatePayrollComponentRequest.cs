using MediatR;
using Hr.Domain.Enums;

namespace Hr.Application.Features.PayrollComponentFeatures.CreatePayrollComponent
{
    public class CreatePayrollComponentRequest : IRequest<CreatePayrollComponentResponse>
    {
        public int PayrollRecordId { get; set; }
        public string Name { get; set; } = string.Empty;
        public PayrollComponentType ComponentType { get; set; }
        public decimal Amount { get; set; }
    }
}
