using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.RecruitmentStageFeatures.DeactivateRecruitmentStage
{
    public class DeactivateRecruitmentStageHandler : IRequestHandler<DeactivateRecruitmentStageRequest, DeactivateRecruitmentStageResponse>
    {
        private readonly IRecruitmentStageRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeactivateRecruitmentStageHandler(IRecruitmentStageRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeactivateRecruitmentStageResponse> Handle(DeactivateRecruitmentStageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var stage = await _repository.GetByIdAsync(request.StageId);
                if (stage == null)
                {
                    return new DeactivateRecruitmentStageResponse
                    {
                        Success = false,
                        Message = "لم يتم العثور على مرحلة التوظيف"
                    };
                }

                stage.IsActive = false;
                _repository.Update(stage);
                await _unitOfWork.SaveChangesAsync();

                return new DeactivateRecruitmentStageResponse
                {
                    Success = true,
                    Message = "تم إلغاء تفعيل مرحلة التوظيف بنجاح"
                };
            }
            catch (Exception ex)
            {
                return new DeactivateRecruitmentStageResponse
                {
                    Success = false,
                    Message = "حدث خطأ أثناء إلغاء تفعيل مرحلة التوظيف"
                };
            }
        }
    }
}
