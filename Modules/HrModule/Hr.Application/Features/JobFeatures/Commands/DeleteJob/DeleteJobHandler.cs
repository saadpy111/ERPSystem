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
