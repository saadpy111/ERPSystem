using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using MediatR;

namespace Hr.Application.Features.ApplicantFeatures.GetApplicantsByJob
{
    public class GetApplicantsByJobHandler : IRequestHandler<GetApplicantsByJobRequest, GetApplicantsByJobResponse>
    {
        private readonly IApplicantRepository _repository;
        private readonly IMapper _mapper;

        public GetApplicantsByJobHandler(IApplicantRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetApplicantsByJobResponse> Handle(GetApplicantsByJobRequest request, CancellationToken cancellationToken)
        {
            var applicants = (await _repository.GetAllAsync())
                .Where(a => a.JobId == request.JobId)
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

            return new GetApplicantsByJobResponse
            {
                Applicants = applicantDtos
            };
        }
    }
}
