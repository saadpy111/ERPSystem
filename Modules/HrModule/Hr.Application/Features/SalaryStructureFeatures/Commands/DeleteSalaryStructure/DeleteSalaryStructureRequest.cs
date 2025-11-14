using MediatR;

namespace Hr.Application.Features.SalaryStructureFeatures.Commands.DeleteSalaryStructure
{
    public class DeleteSalaryStructureRequest : IRequest<DeleteSalaryStructureResponse>
    {
        public int Id { get; set; }
    }
}