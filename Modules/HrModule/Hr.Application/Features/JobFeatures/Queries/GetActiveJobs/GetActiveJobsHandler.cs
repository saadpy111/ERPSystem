using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using MediatR;

namespace Hr.Application.Features.JobFeatures.GetActiveJobs
{
    public class GetActiveJobsHandler : IRequestHandler<GetActiveJobsRequest, GetActiveJobsResponse>
    {
        private readonly IJobRepository _repository;
        private readonly IMapper _mapper;

        public GetActiveJobsHandler(IJobRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetActiveJobsResponse> Handle(GetActiveJobsRequest request, CancellationToken cancellationToken)
        {
            var jobs = (await _repository.GetAllAsync())
                .Where(j => j.IsActive)
                .OrderByDescending(j => j.PublishedDate)
                .ToList();

            var jobDtos = jobs.Select(j => new JobDto
            {
                JobId = j.JobId,
                Title = j.Title,
                DepartmentId = j.DepartmentId,
                DepartmentName = j.Department?.Name ?? string.Empty,
                WorkType = j.WorkType,
                Status = j.Status,
                PublishedDate = j.PublishedDate,
                IsActive = j.IsActive,
                ApplicantsCount = j.Applicants?.Count ?? 0,
                Responsibilities = j.Responsibilities,
                RequiredSkills = j.RequiredSkills,
                RequiredExperience = j.RequiredExperience,
                RequiredQualification = j.RequiredQualification
            });

            return new GetActiveJobsResponse
            {
                Jobs = jobDtos
            };
        }
    }
}