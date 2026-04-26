using MediatR;
using Website.Application.Features.Testimonials.DTOs;
using Website.Application.Pagination;

namespace Website.Application.Features.Testimonials.Queries.GetVisibleTestimonials
{
    public class GetVisibleTestimonialsQuery : IRequest<PagedResult<TestimonialDto>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
