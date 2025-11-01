using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.RecruitmentStageFeatures.GetRecruitmentStageById
{
    public class GetRecruitmentStageByIdHandler : IRequestHandler<GetRecruitmentStageByIdRequest, GetRecruitmentStageByIdResponse>
    {
        private readonly IRecruitmentStageRepository _recruitmentStageRepository;
        private readonly IMapper _mapper;

        public GetRecruitmentStageByIdHandler(IRecruitmentStageRepository recruitmentStageRepository, IMapper mapper)
        {
            _recruitmentStageRepository = recruitmentStageRepository;
            _mapper = mapper;
        }

        public async Task<GetRecruitmentStageByIdResponse> Handle(GetRecruitmentStageByIdRequest request, CancellationToken cancellationToken)
        {
            var recruitmentStage = await _recruitmentStageRepository.GetByIdAsync(request.Id);
            var recruitmentStageDto = _mapper.Map<DTOs.RecruitmentStageDto>(recruitmentStage);

            return new GetRecruitmentStageByIdResponse
            {
                RecruitmentStage = recruitmentStageDto
            };
        }
    }
}
