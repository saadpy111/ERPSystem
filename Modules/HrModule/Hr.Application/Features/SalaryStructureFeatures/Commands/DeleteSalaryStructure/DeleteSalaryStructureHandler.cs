using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hr.Application.Features.SalaryStructureFeatures.Commands.DeleteSalaryStructure
{
    public class DeleteSalaryStructureHandler : IRequestHandler<DeleteSalaryStructureRequest, DeleteSalaryStructureResponse>
    {
        private readonly ISalaryStructureRepository _salaryStructureRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteSalaryStructureHandler(
            ISalaryStructureRepository salaryStructureRepository,
            IUnitOfWork unitOfWork)
        {
            _salaryStructureRepository = salaryStructureRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteSalaryStructureResponse> Handle(DeleteSalaryStructureRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var salaryStructure = await _salaryStructureRepository.GetByIdAsync(request.Id);
                if (salaryStructure == null)
                {
                    return new DeleteSalaryStructureResponse
                    {
                        Success = false,
                        Message = "لم يتم العثور على هيكل الرواتب"
                    };
                }

                _salaryStructureRepository.Delete(salaryStructure);
                await _unitOfWork.SaveChangesAsync();

                return new DeleteSalaryStructureResponse
                {
                    Success = true,
                    Message = "تم حذف هيكل الرواتب بنجاح"
                };
            }
            catch (Exception ex)
            {
                return new DeleteSalaryStructureResponse
                {
                    Success = false,
                    Message = "حدث خطأ أثناء حذف هيكل الرواتب"
                };
            }
        }
    }
}