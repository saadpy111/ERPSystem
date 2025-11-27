using AutoMapper;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using Hr.Domain.Enums;
using MediatR;

namespace Hr.Application.Features.JobFeatures.CreateJob
{
    public class CreateJobHandler : IRequestHandler<CreateJobRequest, CreateJobResponse>
    {
        private readonly IJobRepository _jobRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateJobHandler(IJobRepository jobRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _jobRepository = jobRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateJobResponse> Handle(CreateJobRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var job = new Job
                {
                    Title = request.Title,
                    DepartmentId = request.DepartmentId,
                    WorkType = request.WorkType,
                    PublishedDate = request.PublishedDate,
                    Status = JobStatus.Open,
                    Responsibilities = request.Responsibilities,
                    RequiredSkills = request.RequiredSkills,
                    RequiredExperience = request.RequiredExperience,
                    RequiredQualification = request.RequiredQualification
                };

                await _jobRepository.AddAsync(job);
                await _unitOfWork.SaveChangesAsync();

                var jobDto = _mapper.Map<DTOs.JobDto>(job);

                return new CreateJobResponse
                {
                    Success = true,
                    Message = "تم إنشاء الوظيفة بنجاح",
                    Job = jobDto
                };
            }
            catch (Exception ex)
            {
                return new CreateJobResponse
                {
                    Success = false,
                    Message = "حدث خطأ أثناء إنشاء الوظيفة"
                };
            }
        }
    }
}