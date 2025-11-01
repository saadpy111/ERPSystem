using Hr.Domain.Enums;
using MediatR;

namespace Hr.Application.Features.PayrollComponentFeatures.UpdatePayrollComponent
{
    public class UpdatePayrollComponentRequest : IRequest<UpdatePayrollComponentResponse>
    {
        public int ComponentId { get; set; }
        public int PayrollRecordId { get; set; }
        public string Name { get; set; } = string.Empty;
        public PayrollComponentType ComponentType { get; set; }
        public decimal Amount { get; set; }
    }
}
