using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Website.Application.Contracts.Persistence;

namespace Website.Application.Features.Testimonials.Commands.UpdateVisibility
{
    public class UpdateVisibilityCommandHandler : IRequestHandler<UpdateVisibilityCommand, TestimonialCommandResponse>
    {
        private readonly ITestimonialRepository _repository;
        private readonly IWebsiteUnitOfWork _unitOfWork;

        public UpdateVisibilityCommandHandler(ITestimonialRepository repository, IWebsiteUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TestimonialCommandResponse> Handle(UpdateVisibilityCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == System.Guid.Empty)
                return new TestimonialCommandResponse { Success = false, Error = "Id is required." };

            var testimonial = await _repository.GetByIdAsync(request.Id);
            if (testimonial == null)
                return new TestimonialCommandResponse { Success = false, Error = "Testimonial not found." };

            testimonial.IsVisible = !testimonial.IsVisible;

            await _repository.UpdateAsync(testimonial);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TestimonialCommandResponse { Success = true };
        }
    }
}
