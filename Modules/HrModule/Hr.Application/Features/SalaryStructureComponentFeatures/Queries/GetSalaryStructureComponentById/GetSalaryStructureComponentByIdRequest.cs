using MediatR;

namespace Hr.Application.Features.SalaryStructureComponentFeatures.Queries.GetSalaryStructureComponentById
{
    public class GetSalaryStructureComponentByIdRequest : IRequest<GetSalaryStructureComponentByIdResponse>
    {
        public int Id { get; set; }
    }
}