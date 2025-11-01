# PowerShell script to create remaining CRUD files for HR Module

$basePath = "e:\ERPQoder\ERPSystem\Modules\HrModule\Hr.Application\Features"

# Create Job Update/Delete files
$jobUpdateResponsePath = "$basePath\JobFeatures\UpdateJob"
New-Item -ItemType Directory -Path $jobUpdateResponsePath -Force | Out-Null

@"
using Hr.Application.DTOs;

namespace Hr.Application.Features.JobFeatures.UpdateJob
{
    public class UpdateJobResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public JobDto? Job { get; set; }
    }
}
"@ | Out-File -FilePath "$jobUpdateResponsePath\UpdateJobResponse.cs" -Encoding UTF8

@"
using AutoMapper;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.JobFeatures.UpdateJob
{
    public class UpdateJobHandler : IRequestHandler<UpdateJobRequest, UpdateJobResponse>
    {
        private readonly IJobRepository _jobRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateJobHandler(IJobRepository jobRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _jobRepository = jobRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UpdateJobResponse> Handle(UpdateJobRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var job = await _jobRepository.GetByIdAsync(request.Id);
                if (job == null)
                {
                    return new UpdateJobResponse
                    {
                        Success = false,
                        Message = "Job not found"
                    };
                }

                job.Title = request.Title;
                job.DepartmentId = request.DepartmentId;
                job.WorkType = request.WorkType;
                job.PublishedDate = request.PublishedDate;
                job.Status = request.Status;

                _jobRepository.Update(job);
                await _unitOfWork.SaveChangesAsync();

                var jobDto = _mapper.Map<DTOs.JobDto>(job);

                return new UpdateJobResponse
                {
                    Success = true,
                    Message = "Job updated successfully",
                    Job = jobDto
                };
            }
            catch (Exception ex)
            {
                return new UpdateJobResponse
                {
                    Success = false,
                    Message = $"Error updating job: {ex.Message}"
                };
            }
        }
    }
}
"@ | Out-File -FilePath "$jobUpdateResponsePath\UpdateJobHandler.cs" -Encoding UTF8

$jobDeletePath = "$basePath\JobFeatures\DeleteJob"

@"
namespace Hr.Application.Features.JobFeatures.DeleteJob
{
    public class DeleteJobResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
"@ | Out-File -FilePath "$jobDeletePath\DeleteJobResponse.cs" -Encoding UTF8

@"
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.JobFeatures.DeleteJob
{
    public class DeleteJobHandler : IRequestHandler<DeleteJobRequest, DeleteJobResponse>
    {
        private readonly IJobRepository _jobRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteJobHandler(IJobRepository jobRepository, IUnitOfWork unitOfWork)
        {
            _jobRepository = jobRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteJobResponse> Handle(DeleteJobRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var job = await _jobRepository.GetByIdAsync(request.Id);
                if (job == null)
                {
                    return new DeleteJobResponse
                    {
                        Success = false,
                        Message = "Job not found"
                    };
                }

                _jobRepository.Delete(job);
                await _unitOfWork.SaveChangesAsync();

                return new DeleteJobResponse
                {
                    Success = true,
                    Message = "Job deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new DeleteJobResponse
                {
                    Success = false,
                    Message = $"Error deleting job: {ex.Message}"
                };
            }
        }
    }
}
"@ | Out-File -FilePath "$jobDeletePath\DeleteJobHandler.cs" -Encoding UTF8

Write-Host "Job files created successfully!"
