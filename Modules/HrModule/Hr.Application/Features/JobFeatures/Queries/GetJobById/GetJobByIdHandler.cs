using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.JobFeatures.GetJobById
{
    public class GetJobByIdHandler : IRequestHandler<GetJobByIdRequest, GetJobByIdResponse>
    {
        private readonly IJobRepository _jobRepository;
        private readonly IMapper _mapper;

        public GetJobByIdHandler(IJobRepository jobRepository, IMapper mapper)
        {
            _jobRepository = jobRepository;
            _mapper = mapper;
        }

        public async Task<GetJobByIdResponse> Handle(GetJobByIdRequest request, CancellationToken cancellationToken)
        {
            var job = await _jobRepository.GetByIdAsync(request.Id);
            var jobDto = _mapper.Map<DTOs.JobDto>(job);

            return new GetJobByIdResponse
            {
                Job = jobDto
            };
        }
    }
}
