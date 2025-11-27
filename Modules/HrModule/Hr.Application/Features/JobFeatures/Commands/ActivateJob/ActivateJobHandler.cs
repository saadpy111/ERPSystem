using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.JobFeatures.ActivateJob
{
    public class ActivateJobHandler : IRequestHandler<ActivateJobRequest, ActivateJobResponse>
    {
        private readonly IJobRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ActivateJobHandler(IJobRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ActivateJobResponse> Handle(ActivateJobRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var job = await _repository.GetByIdAsync(request.JobId);
                if (job == null)
                {
                    return new ActivateJobResponse
                    {
                        Success = false,
                        Message = "الوظيفة غير موجودة"
                    };
                }

                job.IsActive = true;
                _repository.Update(job);
                await _unitOfWork.SaveChangesAsync();

                return new ActivateJobResponse
                {
                    Success = true,
                    Message = "تم تفعيل الوظيفة بنجاح"
                };
            }
            catch (Exception ex)
            {
                return new ActivateJobResponse
                {
                    Success = false,
                    Message = "حدث خطأ أثناء تفعيل الوظيفة"
                };
            }
        }
    }
}
