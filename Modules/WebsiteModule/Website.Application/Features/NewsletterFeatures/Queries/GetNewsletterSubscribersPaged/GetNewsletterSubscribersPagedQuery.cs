using MediatR;
using Website.Application.DTOs;
using Website.Application.Pagination;

namespace Website.Application.Features.NewsletterFeatures.Queries.GetNewsletterSubscribersPaged
{
    public class GetNewsletterSubscribersPagedQuery : IRequest<PagedResult<NewsletterSubscriberDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; }
        public bool? IsActive { get; set; }
    }
}
