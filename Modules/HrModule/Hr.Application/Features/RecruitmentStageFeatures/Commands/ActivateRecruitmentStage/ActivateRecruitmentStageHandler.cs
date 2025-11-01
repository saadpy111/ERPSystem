using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.RecruitmentStageFeatures.ActivateRecruitmentStage
{
    public class ActivateRecruitmentStageHandler : IRequestHandler<ActivateRecruitmentStageRequest, ActivateRecruitmentStageResponse>
    {
        private readonly IRecruitmentStageRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ActivateRecruitmentStageHandler(IRecruitmentStageRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ActivateRecruitmentStageResponse> Handle(ActivateRecruitmentStageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var stage = await _repository.GetByIdAsync(request.StageId);
                if (stage == null)
                {
                    return new ActivateRecruitmentStageResponse
                    {
                        Success = false,
                        Message = "Recruitment stage not found"
                    };
                }

                stage.IsActive = true;
                _repository.Update(stage);
                await _unitOfWork.SaveChangesAsync();

                return new ActivateRecruitmentStageResponse
                {
                    Success = true,
                    Message = "Recruitment stage activated successfully"
                };
            }
            catch (Exception ex)
            {
                return new ActivateRecruitmentStageResponse
                {
                    Success = false,
                    Message = $"Error activating recruitment stage: {ex.Message}"
                };
            }
        }
    }
}
