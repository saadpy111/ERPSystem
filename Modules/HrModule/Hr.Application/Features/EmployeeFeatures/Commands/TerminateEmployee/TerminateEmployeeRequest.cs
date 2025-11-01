using MediatR;

namespace Hr.Application.Features.EmployeeFeatures.TerminateEmployee
{
    public class TerminateEmployeeRequest : IRequest<TerminateEmployeeResponse>
    {
        public int EmployeeId { get; set; }
    }
}
