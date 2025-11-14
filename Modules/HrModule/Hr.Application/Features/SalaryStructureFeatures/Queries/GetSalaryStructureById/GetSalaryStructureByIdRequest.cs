using MediatR;

namespace Hr.Application.Features.SalaryStructureFeatures.Queries.GetSalaryStructureById
{
    public class GetSalaryStructureByIdRequest : IRequest<GetSalaryStructureByIdResponse>
    {
        public int Id { get; set; }
    }
}