using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.RecruitmentStageFeatures.DeleteRecruitmentStage
{
    public class DeleteRecruitmentStageHandler : IRequestHandler<DeleteRecruitmentStageRequest, DeleteRecruitmentStageResponse>
    {
        private readonly IRecruitmentStageRepository _recruitmentStageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteRecruitmentStageHandler(IRecruitmentStageRepository recruitmentStageRepository, IUnitOfWork unitOfWork)
        {
            _recruitmentStageRepository = recruitmentStageRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteRecruitmentStageResponse> Handle(DeleteRecruitmentStageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var recruitmentStage = await _recruitmentStageRepository.GetByIdAsync(request.StageId);
                if (recruitmentStage == null)
                {
                    return new DeleteRecruitmentStageResponse
                    {
                        Success = false,
                        Message = "Recruitment stage not found"
                    };
                }

                _recruitmentStageRepository.Delete(recruitmentStage);
                await _unitOfWork.SaveChangesAsync();

                return new DeleteRecruitmentStageResponse
                {
                    Success = true,
                    Message = "Recruitment stage deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new DeleteRecruitmentStageResponse
                {
                    Success = false,
                    Message = $"Error deleting recruitment stage: {ex.Message}"
                };
            }
        }
    }
}
