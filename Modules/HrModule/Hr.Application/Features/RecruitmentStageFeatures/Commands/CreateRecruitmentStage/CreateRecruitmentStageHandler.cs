using AutoMapper;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using MediatR;

namespace Hr.Application.Features.RecruitmentStageFeatures.CreateRecruitmentStage
{
    public class CreateRecruitmentStageHandler : IRequestHandler<CreateRecruitmentStageRequest, CreateRecruitmentStageResponse>
    {
        private readonly IRecruitmentStageRepository _recruitmentStageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateRecruitmentStageHandler(IRecruitmentStageRepository recruitmentStageRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _recruitmentStageRepository = recruitmentStageRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateRecruitmentStageResponse> Handle(CreateRecruitmentStageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var recruitmentStage = new RecruitmentStage
                {
                    Name = request.Name,
                    SequenceOrder = request.SequenceOrder
                };

                await _recruitmentStageRepository.AddAsync(recruitmentStage);
                await _unitOfWork.SaveChangesAsync();

                var recruitmentStageDto = _mapper.Map<DTOs.RecruitmentStageDto>(recruitmentStage);

                return new CreateRecruitmentStageResponse
                {
                    Success = true,
                    Message = "Recruitment stage created successfully",
                    RecruitmentStage = recruitmentStageDto
                };
            }
            catch (Exception ex)
            {
                return new CreateRecruitmentStageResponse
                {
                    Success = false,
                    Message = $"Error creating recruitment stage: {ex.Message}"
                };
            }
        }
    }
}
