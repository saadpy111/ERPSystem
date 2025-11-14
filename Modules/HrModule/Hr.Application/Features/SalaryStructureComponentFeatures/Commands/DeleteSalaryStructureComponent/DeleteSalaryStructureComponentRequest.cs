using MediatR;

namespace Hr.Application.Features.SalaryStructureComponentFeatures.Commands.DeleteSalaryStructureComponent
{
    public class DeleteSalaryStructureComponentRequest : IRequest<DeleteSalaryStructureComponentResponse>
    {
        public int Id { get; set; }
    }
}