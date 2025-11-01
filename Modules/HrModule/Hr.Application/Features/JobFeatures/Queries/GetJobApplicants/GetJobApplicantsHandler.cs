using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using MediatR;

namespace Hr.Application.Features.JobFeatures.GetJobApplicants
{
    public class GetJobApplicantsHandler : IRequestHandler<GetJobApplicantsRequest, GetJobApplicantsResponse>
    {
        private readonly IJobRepository _jobRepository;
        private readonly IApplicantRepository _applicantRepository;

        public GetJobApplicantsHandler(IJobRepository jobRepository, IApplicantRepository applicantRepository)
        {
            _jobRepository = jobRepository;
            _applicantRepository = applicantRepository;
        }

        public async Task<GetJobApplicantsResponse> Handle(GetJobApplicantsRequest request, CancellationToken cancellationToken)
        {
            var job = await _jobRepository.GetByIdAsync(request.JobId);
            if (job == null)
            {
                return new GetJobApplicantsResponse
                {
                    JobId = request.JobId,
                    JobTitle = string.Empty,
                    Applicants = new List<ApplicantDto>()
                };
            }

            var applicants = await _applicantRepository.GetByJobIdAsync(request.JobId);

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
            }).OrderByDescending(a => a.ApplicationDate);

            return new GetJobApplicantsResponse
            {
                JobId = job.JobId,
                JobTitle = job.Title,
                Applicants = applicantDtos
            };
        }
    }
}
