using MediatR;

namespace Hr.Application.Features.EmployeeContractFeatures.Commands.DeleteEmployeeContract
{
    public class DeleteEmployeeContractRequest : IRequest<DeleteEmployeeContractResponse>
    {
        public int Id { get; set; }
    }
}