using MediatR;

namespace Hr.Application.Features.EmployeeFeatures.DeleteEmployee
{
    public class DeleteEmployeeRequest : IRequest<DeleteEmployeeResponse>
    {
        public int Id { get; set; }
    }
}
