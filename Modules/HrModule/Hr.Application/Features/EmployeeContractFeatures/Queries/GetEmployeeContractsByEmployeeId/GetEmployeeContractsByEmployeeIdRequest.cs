using MediatR;

namespace Hr.Application.Features.EmployeeContractFeatures.Queries.GetEmployeeContractsByEmployeeId
{
    public class GetEmployeeContractsByEmployeeIdRequest : IRequest<GetEmployeeContractsByEmployeeIdResponse>
    {
        public int EmployeeId { get; set; }
    }
}