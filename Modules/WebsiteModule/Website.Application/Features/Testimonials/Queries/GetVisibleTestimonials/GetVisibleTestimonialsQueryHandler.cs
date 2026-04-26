using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Website.Application.Contracts.Persistence;
using Website.Application.Features.Testimonials.DTOs;
using Website.Application.Pagination;

namespace Website.Application.Features.Testimonials.Queries.GetVisibleTestimonials
{
    public class GetVisibleTestimonialsQueryHandler : IRequestHandler<GetVisibleTestimonialsQuery, PagedResult<TestimonialDto>>
    {
        private readonly ITestimonialRepository _repository;

        public GetVisibleTestimonialsQueryHandler(ITestimonialRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<TestimonialDto>> Handle(GetVisibleTestimonialsQuery request, CancellationToken cancellationToken)
        {
            var (items, totalCount) = await _repository.GetVisiblePagedAsync(request.Page, request.PageSize);

            var dtos = items.Select(t => new TestimonialDto
            {
                Id = t.Id,
                CustomerName = t.CustomerName,
                Comment = t.Comment,
                Rating = t.Rating,
                IsVisible = t.IsVisible,
                Order = t.Order
            }).ToList();

            return new PagedResult<TestimonialDto>
            {
                Items = dtos,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}
