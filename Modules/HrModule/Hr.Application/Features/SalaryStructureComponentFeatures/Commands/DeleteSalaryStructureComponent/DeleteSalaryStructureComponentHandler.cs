using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hr.Application.Features.SalaryStructureComponentFeatures.Commands.DeleteSalaryStructureComponent
{
    public class DeleteSalaryStructureComponentHandler : IRequestHandler<DeleteSalaryStructureComponentRequest, DeleteSalaryStructureComponentResponse>
    {
        private readonly ISalaryStructureComponentRepository _salaryStructureComponentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteSalaryStructureComponentHandler(
            ISalaryStructureComponentRepository salaryStructureComponentRepository,
            IUnitOfWork unitOfWork)
        {
            _salaryStructureComponentRepository = salaryStructureComponentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteSalaryStructureComponentResponse> Handle(DeleteSalaryStructureComponentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var salaryStructureComponent = await _salaryStructureComponentRepository.GetByIdAsync(request.Id);
                if (salaryStructureComponent == null)
                {
                    return new DeleteSalaryStructureComponentResponse
                    {
                        Success = false,
                        Message = "Salary structure component not found"
                    };
                }

                _salaryStructureComponentRepository.Delete(salaryStructureComponent);
                await _unitOfWork.SaveChangesAsync();

                return new DeleteSalaryStructureComponentResponse
                {
                    Success = true,
                    Message = "Salary structure component deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new DeleteSalaryStructureComponentResponse
                {
                    Success = false,
                    Message = $"Error deleting salary structure component: {ex.Message}"
                };
            }
        }
    }
}