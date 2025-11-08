using MediatR;

namespace Hr.Application.Features.EmployeeFeatures.PromoteEmployee
{
    public class PromoteEmployeeRequest : IRequest<PromoteEmployeeResponse>
    {
        public int EmployeeId { get; set; }
    }
}
