using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using MediatR;

namespace Hr.Application.Features.RecruitmentStageFeatures.GetActiveRecruitmentStages
{
    public class GetActiveRecruitmentStagesHandler : IRequestHandler<GetActiveRecruitmentStagesRequest, GetActiveRecruitmentStagesResponse>
    {
        private readonly IRecruitmentStageRepository _repository;
        private readonly IMapper _mapper;

        public GetActiveRecruitmentStagesHandler(IRecruitmentStageRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetActiveRecruitmentStagesResponse> Handle(GetActiveRecruitmentStagesRequest request, CancellationToken cancellationToken)
        {
            var stages = (await _repository.GetAllAsync())
                .Where(s => s.IsActive)
                .OrderBy(s => s.SequenceOrder)
                .ToList();

            var stageDtos = stages.Select(s => new RecruitmentStageDto
            {
                StageId = s.StageId,
                Name = s.Name,
                SequenceOrder = s.SequenceOrder,
                IsActive = s.IsActive
            });

            return new GetActiveRecruitmentStagesResponse
            {
                Stages = stageDtos
            };
        }
    }
}
