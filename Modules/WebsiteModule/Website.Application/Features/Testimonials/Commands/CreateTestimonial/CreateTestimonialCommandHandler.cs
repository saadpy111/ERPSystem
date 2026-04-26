using MediatR;
using SharedKernel.Multitenancy;
using System.Threading;
using System.Threading.Tasks;
using Website.Application.Contracts.Persistence;
using Website.Domain.Entities;

namespace Website.Application.Features.Testimonials.Commands.CreateTestimonial
{
    public class CreateTestimonialCommandHandler : IRequestHandler<CreateTestimonialCommand, TestimonialCommandResponse>
    {
        private readonly ITestimonialRepository _repository;
        private readonly ITenantProvider _tenantProvider;
        private readonly IWebsiteUnitOfWork _unitOfWork;



        public CreateTestimonialCommandHandler(ITestimonialRepository repository,ITenantProvider tenantProvider, IWebsiteUnitOfWork unitOfWork)
        {
            _repository = repository;
            _tenantProvider = tenantProvider;
            _unitOfWork = unitOfWork;
        }

        public async Task<TestimonialCommandResponse> Handle(CreateTestimonialCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.CustomerName))
                return new TestimonialCommandResponse { Success = false, Error = "Customer name is required." };

            if (string.IsNullOrWhiteSpace(request.Comment))
                return new TestimonialCommandResponse { Success = false, Error = "Comment is required." };

            if (request.Rating < 1 || request.Rating > 5)
                return new TestimonialCommandResponse { Success = false, Error = "Rating must be between 1 and 5." };

            var maxOrder = await _repository.GetMaxOrderAsync();

            var testimonial = new Testimonial
            {
                CustomerName = request.CustomerName,
                Comment = request.Comment,
                Rating = request.Rating,
                IsVisible = true,
                Order = maxOrder + 1,
                TenantId = _tenantProvider.GetTenantId()!
            };

            await _repository.AddAsync(testimonial);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TestimonialCommandResponse { Success = true };
        }
    }
}
