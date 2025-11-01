using MediatR;

namespace Hr.Application.Features.EmployeeFeatures.ActivateEmployee
{
    public class ActivateEmployeeRequest : IRequest<ActivateEmployeeResponse>
    {
        public int EmployeeId { get; set; }
    }
}


