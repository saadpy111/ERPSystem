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
                        Message = "لم يتم العثور على مكون هيكل الرواتب"
                    };
                }

                _salaryStructureComponentRepository.Delete(salaryStructureComponent);
                await _unitOfWork.SaveChangesAsync();

                return new DeleteSalaryStructureComponentResponse
                {
                    Success = true,
                    Message = "تم حذف مكون هيكل الرواتب بنجاح"
                };
            }
            catch (Exception ex)
            {
                return new DeleteSalaryStructureComponentResponse
                {
                    Success = false,
                    Message = "حدث خطأ أثناء حذف مكون هيكل الرواتب"
                };
            }
        }
    }
}