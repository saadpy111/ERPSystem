using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Website.Application.Contracts.Persistence;

namespace Website.Application.Features.Testimonials.Commands.UpdateTestimonial
{
    public class UpdateTestimonialCommandHandler : IRequestHandler<UpdateTestimonialCommand, TestimonialCommandResponse>
    {
        private readonly ITestimonialRepository _repository;
        private readonly IWebsiteUnitOfWork _unitOfWork;

        public UpdateTestimonialCommandHandler(ITestimonialRepository repository, IWebsiteUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TestimonialCommandResponse> Handle(UpdateTestimonialCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == System.Guid.Empty)
                return new TestimonialCommandResponse { Success = false, Error = "Id is required." };

            if (string.IsNullOrWhiteSpace(request.CustomerName))
                return new TestimonialCommandResponse { Success = false, Error = "Customer name is required." };

            if (string.IsNullOrWhiteSpace(request.Comment))
                return new TestimonialCommandResponse { Success = false, Error = "Comment is required." };

            if (request.Rating < 1 || request.Rating > 5)
                return new TestimonialCommandResponse { Success = false, Error = "Rating must be between 1 and 5." };

            var testimonial = await _repository.GetByIdAsync(request.Id);
            if (testimonial == null)
                return new TestimonialCommandResponse { Success = false, Error = "Testimonial not found." };

            testimonial.CustomerName = request.CustomerName;
            testimonial.Comment = request.Comment;
            testimonial.Rating = request.Rating;

            await _repository.UpdateAsync(testimonial);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TestimonialCommandResponse { Success = true };
        }
    }
}
