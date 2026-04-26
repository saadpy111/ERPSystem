using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Website.Application.Contracts.Persistence;
using Website.Application.Features.Testimonials.DTOs;

namespace Website.Application.Features.Testimonials.Queries.GetTestimonialById
{
    public class GetTestimonialByIdQueryHandler : IRequestHandler<GetTestimonialByIdQuery, TestimonialDto?>
    {
        private readonly ITestimonialRepository _repository;

        public GetTestimonialByIdQueryHandler(ITestimonialRepository repository)
        {
            _repository = repository;
        }

        public async Task<TestimonialDto?> Handle(GetTestimonialByIdQuery request, CancellationToken cancellationToken)
        {
            var testimonial = await _repository.GetByIdAsync(request.Id);

            if (testimonial == null)
                return null;

            return new TestimonialDto
            {
                Id = testimonial.Id,
                CustomerName = testimonial.CustomerName,
                Comment = testimonial.Comment,
                Rating = testimonial.Rating,
                IsVisible = testimonial.IsVisible,
                Order = testimonial.Order
            };
        }
    }
}
