using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using MediatR;

namespace Hr.Application.Features.RecruitmentStageFeatures.ReorderRecruitmentStages
{
    public class ReorderRecruitmentStagesHandler : IRequestHandler<ReorderRecruitmentStagesRequest, ReorderRecruitmentStagesResponse>
    {
        private readonly IRecruitmentStageRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ReorderRecruitmentStagesHandler(IRecruitmentStageRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ReorderRecruitmentStagesResponse> Handle(ReorderRecruitmentStagesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.StageOrders == null || !request.StageOrders.Any())
                {
                    return new ReorderRecruitmentStagesResponse
                    {
                        Success = false,
                        Message = "لم يتم تقديم ترتيب المراحل"
                    };
                }

                var allStages = await _repository.GetAllAsync();
                var updatedStages = new List<RecruitmentStageDto>();

                foreach (var stageOrder in request.StageOrders)
                {
                    var stage = allStages.FirstOrDefault(s => s.StageId == stageOrder.StageId);
                    if (stage != null)
                    {
                        stage.SequenceOrder = stageOrder.SequenceOrder;
                        _repository.Update(stage);

                        updatedStages.Add(new RecruitmentStageDto
                        {
                            StageId = stage.StageId,
                            Name = stage.Name,
                            SequenceOrder = stage.SequenceOrder,
                            IsActive = stage.IsActive
                        });
                    }
                }

                await _unitOfWork.SaveChangesAsync();

                return new ReorderRecruitmentStagesResponse
                {
                    Success = true,
                    Message = "تم إعادة ترتيب مراحل التوظيف بنجاح",
                    ReorderedStages = updatedStages.OrderBy(s => s.SequenceOrder)
                };
            }
            catch (Exception ex)
            {
                return new ReorderRecruitmentStagesResponse
                {
                    Success = false,
                    Message = "حدث خطأ أثناء إعادة ترتيب مراحل التوظيف"
                };
            }
        }
    }
}
