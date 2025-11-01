using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using MediatR;

namespace Hr.Application.Features.ApplicantFeatures.GetApplicantsByStage
{
    public class GetApplicantsByStageHandler : IRequestHandler<GetApplicantsByStageRequest, GetApplicantsByStageResponse>
    {
        private readonly IApplicantRepository _repository;
        private readonly IMapper _mapper;

        public GetApplicantsByStageHandler(IApplicantRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetApplicantsByStageResponse> Handle(GetApplicantsByStageRequest request, CancellationToken cancellationToken)
        {
            var applicants = (await _repository.GetAllAsync())
                .Where(a => a.CurrentStageId == request.StageId)
                .OrderByDescending(a => a.ApplicationDate)
                .ToList();

            var applicantDtos = applicants.Select(a => new ApplicantDto
            {
                ApplicantId = a.ApplicantId,
                FullName = a.FullName,
                JobId = a.JobId,
                JobTitle = a.AppliedJob?.Title ?? string.Empty,
                ApplicationDate = a.ApplicationDate,
                CurrentStageId = a.CurrentStageId,
                CurrentStageName = a.CurrentStage?.Name ?? string.Empty,
                Status = a.Status,
                ResumeUrl = a.ResumeUrl,
                InterviewDate = a.InterviewDate
            });

            return new GetApplicantsByStageResponse
            {
                Applicants = applicantDtos
            };
        }
    }
}
