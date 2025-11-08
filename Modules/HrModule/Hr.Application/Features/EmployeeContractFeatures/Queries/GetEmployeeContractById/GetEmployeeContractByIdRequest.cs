using MediatR;

namespace Hr.Application.Features.EmployeeContractFeatures.Queries.GetEmployeeContractById
{
    public class GetEmployeeContractByIdRequest : IRequest<GetEmployeeContractByIdResponse>
    {
        public int Id { get; set; }
    }
}