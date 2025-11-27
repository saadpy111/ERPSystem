using AutoMapper;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.RecruitmentStageFeatures.UpdateRecruitmentStage
{
    public class UpdateRecruitmentStageHandler : IRequestHandler<UpdateRecruitmentStageRequest, UpdateRecruitmentStageResponse>
    {
        private readonly IRecruitmentStageRepository _recruitmentStageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateRecruitmentStageHandler(IRecruitmentStageRepository recruitmentStageRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _recruitmentStageRepository = recruitmentStageRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UpdateRecruitmentStageResponse> Handle(UpdateRecruitmentStageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var recruitmentStage = await _recruitmentStageRepository.GetByIdAsync(request.StageId);
                if (recruitmentStage == null)
                {
                    return new UpdateRecruitmentStageResponse
                    {
                        Success = false,
                        Message = "لم يتم العثور على مرحلة التوظيف"
                    };
                }

                recruitmentStage.Name = request.Name;
                recruitmentStage.Description = request.Description;
                recruitmentStage.SequenceOrder = request.SequenceOrder;

                _recruitmentStageRepository.Update(recruitmentStage);
                await _unitOfWork.SaveChangesAsync();

                var recruitmentStageDto = _mapper.Map<DTOs.RecruitmentStageDto>(recruitmentStage);

                return new UpdateRecruitmentStageResponse
                {
                    Success = true,
                    Message = "تم تحديث مرحلة التوظيف بنجاح",
                    RecruitmentStage = recruitmentStageDto
                };
            }
            catch (Exception ex)
            {
                return new UpdateRecruitmentStageResponse
                {
                    Success = false,
                    Message = "حدث خطأ أثناء تحديث مرحلة التوظيف"
                };
            }
        }
    }
}
