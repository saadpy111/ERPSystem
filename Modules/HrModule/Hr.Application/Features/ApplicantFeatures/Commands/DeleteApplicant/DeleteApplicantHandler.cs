using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.ApplicantFeatures.DeleteApplicant
{
    public class DeleteApplicantHandler : IRequestHandler<DeleteApplicantRequest, DeleteApplicantResponse>
    {
        private readonly IApplicantRepository _applicantRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteApplicantHandler(IApplicantRepository applicantRepository, IUnitOfWork unitOfWork)
        {
            _applicantRepository = applicantRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteApplicantResponse> Handle(DeleteApplicantRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var applicant = await _applicantRepository.GetByIdAsync(request.ApplicantId);
                if (applicant == null)
                {
                    return new DeleteApplicantResponse
                    {
                        Success = false,
                        Message = "Applicant not found"
                    };
                }

                _applicantRepository.Delete(applicant);
                await _unitOfWork.SaveChangesAsync();

                return new DeleteApplicantResponse
                {
                    Success = true,
                    Message = "Applicant deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new DeleteApplicantResponse
                {
                    Success = false,
                    Message = $"Error deleting applicant: {ex.Message}"
                };
            }
        }
    }
}
