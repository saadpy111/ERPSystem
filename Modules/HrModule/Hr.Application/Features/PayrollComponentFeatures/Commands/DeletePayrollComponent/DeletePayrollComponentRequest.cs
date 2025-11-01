using MediatR;

namespace Hr.Application.Features.PayrollComponentFeatures.DeletePayrollComponent
{
    public class DeletePayrollComponentRequest : IRequest<DeletePayrollComponentResponse>
    {
        public int ComponentId { get; set; }
    }
}
