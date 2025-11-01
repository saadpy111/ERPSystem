using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using MediatR;

namespace Hr.Application.Features.RecruitmentStageFeatures.GetAllRecruitmentStages
{
    public class GetAllRecruitmentStagesHandler : IRequestHandler<GetAllRecruitmentStagesRequest, GetAllRecruitmentStagesResponse>
    {
        private readonly IRecruitmentStageRepository _recruitmentStageRepository;
        private readonly IMapper _mapper;

        public GetAllRecruitmentStagesHandler(IRecruitmentStageRepository recruitmentStageRepository, IMapper mapper)
        {
            _recruitmentStageRepository = recruitmentStageRepository;
            _mapper = mapper;
        }

        public async Task<GetAllRecruitmentStagesResponse> Handle(GetAllRecruitmentStagesRequest request, CancellationToken cancellationToken)
        {
            var recruitmentStages = await _recruitmentStageRepository.GetAllAsync();
            var recruitmentStageDtos = _mapper.Map<IEnumerable<RecruitmentStageDto>>(recruitmentStages);

            return new GetAllRecruitmentStagesResponse
            {
                RecruitmentStages = recruitmentStageDtos
            };
        }
    }
}
