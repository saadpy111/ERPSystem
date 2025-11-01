using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using MediatR;

namespace Hr.Application.Features.RecruitmentStageFeatures.GetOrderedRecruitmentStages
{
    public class GetOrderedRecruitmentStagesHandler : IRequestHandler<GetOrderedRecruitmentStagesRequest, GetOrderedRecruitmentStagesResponse>
    {
        private readonly IRecruitmentStageRepository _repository;
        private readonly IMapper _mapper;

        public GetOrderedRecruitmentStagesHandler(IRecruitmentStageRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetOrderedRecruitmentStagesResponse> Handle(GetOrderedRecruitmentStagesRequest request, CancellationToken cancellationToken)
        {
            var stages = (await _repository.GetAllAsync())
                .OrderBy(s => s.SequenceOrder)
                .ToList();

            var stageDtos = stages.Select(s => new RecruitmentStageDto
            {
                StageId = s.StageId,
                Name = s.Name,
                SequenceOrder = s.SequenceOrder,
                IsActive = s.IsActive
            });

            return new GetOrderedRecruitmentStagesResponse
            {
                Stages = stageDtos
            };
        }
    }
}
