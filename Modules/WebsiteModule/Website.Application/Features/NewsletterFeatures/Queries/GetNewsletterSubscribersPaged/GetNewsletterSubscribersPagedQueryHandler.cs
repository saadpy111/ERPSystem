using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Website.Application.Contracts.Persistence;
using Website.Application.DTOs;
using Website.Application.Pagination;

namespace Website.Application.Features.NewsletterFeatures.Queries.GetNewsletterSubscribersPaged
{
    public class GetNewsletterSubscribersPagedQueryHandler : IRequestHandler<GetNewsletterSubscribersPagedQuery, PagedResult<NewsletterSubscriberDto>>
    {
        private readonly INewsletterSubscriberRepository _repository;

        public GetNewsletterSubscribersPagedQueryHandler(INewsletterSubscriberRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<NewsletterSubscriberDto>> Handle(GetNewsletterSubscribersPagedQuery request, CancellationToken cancellationToken)
        {
            var (items, totalCount) = await _repository.GetPagedAsync(
                request.Search,
                request.IsActive,
                request.PageNumber,
                request.PageSize);

            var dtos = items.Select(s => new NewsletterSubscriberDto
            {
                Id = s.Id,
                Email = s.Email,
                IsActive = s.IsActive,
                SubscribedAt = s.SubscribedAt
            }).ToList();

            return new PagedResult<NewsletterSubscriberDto>
            {
                Items = dtos,
                TotalCount = totalCount,
                Page = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
}
