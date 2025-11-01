using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.JobFeatures.DeactivateJob
{
    public class DeactivateJobHandler : IRequestHandler<DeactivateJobRequest, DeactivateJobResponse>
    {
        private readonly IJobRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeactivateJobHandler(IJobRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeactivateJobResponse> Handle(DeactivateJobRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var job = await _repository.GetByIdAsync(request.JobId);
                if (job == null)
                {
                    return new DeactivateJobResponse
                    {
                        Success = false,
                        Message = "Job not found"
                    };
                }

                job.IsActive = false;
                _repository.Update(job);
                await _unitOfWork.SaveChangesAsync();

                return new DeactivateJobResponse
                {
                    Success = true,
                    Message = "Job deactivated successfully"
                };
            }
            catch (Exception ex)
            {
                return new DeactivateJobResponse
                {
                    Success = false,
                    Message = $"Error deactivating job: {ex.Message}"
                };
            }
        }
    }
}
