using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Website.Application.Contracts.Persistence;

namespace Website.Application.Features.Testimonials.Commands.ReorderTestimonials
{
    public class ReorderTestimonialsCommandHandler : IRequestHandler<ReorderTestimonialsCommand, TestimonialCommandResponse>
    {
        private readonly ITestimonialRepository _repository;
        private readonly IWebsiteUnitOfWork _unitOfWork;

        public ReorderTestimonialsCommandHandler(ITestimonialRepository repository, IWebsiteUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TestimonialCommandResponse> Handle(ReorderTestimonialsCommand request, CancellationToken cancellationToken)
        {
            if (request.OrderedItems == null || !request.OrderedItems.Any())
                return new TestimonialCommandResponse { Success = false, Error = "Ordered items are required." };

            var testimonials = await _repository.GetAllAsync();
            
            foreach (var item in request.OrderedItems)
            {
                var testimonial = testimonials.FirstOrDefault(t => t.Id == item.Id);
                if (testimonial != null)
                {
                    testimonial.Order = item.Order;
                    await _repository.UpdateAsync(testimonial);
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TestimonialCommandResponse { Success = true };
        }
    }
}
