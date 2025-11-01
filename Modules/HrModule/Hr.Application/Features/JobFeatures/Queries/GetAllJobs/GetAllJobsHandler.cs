using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using MediatR;

namespace Hr.Application.Features.JobFeatures.GetAllJobs
{
    public class GetAllJobsHandler : IRequestHandler<GetAllJobsRequest, GetAllJobsResponse>
    {
        private readonly IJobRepository _jobRepository;
        private readonly IMapper _mapper;

        public GetAllJobsHandler(IJobRepository jobRepository, IMapper mapper)
        {
            _jobRepository = jobRepository;
            _mapper = mapper;
        }

        public async Task<GetAllJobsResponse> Handle(GetAllJobsRequest request, CancellationToken cancellationToken)
        {
            var jobs = await _jobRepository.GetAllAsync();
            var jobDtos = _mapper.Map<IEnumerable<JobDto>>(jobs);

            return new GetAllJobsResponse
            {
                Jobs = jobDtos
            };
        }
    }
}
